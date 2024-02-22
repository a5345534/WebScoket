using Microsoft.AspNetCore.Connections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using WebSocketService.Classes;
using WebSocketService.DTOs;
using WebSocketService.Enums;

namespace WebSocketService
{
    public class MyWebSocketManager
    {
        private static ConcurrentDictionary<string, ConnectedUser> _programUsers = new ConcurrentDictionary<string, ConnectedUser>();
        private static Dictionary<string, ChatRoom> _chatRooms = new Dictionary<string, ChatRoom>();
        public async Task HandelNewConnect(HttpContext context)
        {
            var websocketCts = new CancellationTokenSource();
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();//接收WebSocket
            var userName = context.Request.Query["userName"].ToString();
            var newUser = new ConnectedUser(userName, webSocket);//新用戶成立
            _programUsers.TryAdd(newUser.id, newUser); // 將新的使用者加入使用者列表            
            newUser.ListenMessage(new ConnectingUserDto());//將唯一ID傳回前端            
            foreach (var room in _chatRooms)
            {
                newUser.ListenMessage(new CreateChatRoomDto(room.Value.chatRoomId));//將現有房間回傳新用戶
            }
            Task.WaitAll();
            try
            {
                newUser.heartbeatTimer.Elapsed += async (sender, e) => await SendPingTo(newUser);//創造定期發送ping任務
                newUser.heartbeatTimer.Elapsed += async (sender, e) => await newUser.IsHeartStop(websocketCts);
                newUser.heartbeatTimer.Start();
                //持續聆聽
                while (newUser.webSocket.State == WebSocketState.Open)
                {
                    var reciveJson = await newUser.SendJsonMessage(websocketCts); // 从客户端接收消息

                    var masterDto = JsonSerializer.Deserialize<Dictionary<string, object>>(reciveJson);
                    Enum.TryParse<DtoType>(masterDto["dtoType"].ToString(), out var dtoType);
                    switch (dtoType)
                    {
                        case DtoType.ProgramConnect:
                            break;
                        case DtoType.CreateChatRoom:
                            await CreateChatRoom();
                            Console.WriteLine($"{newUser.name}創建聊天室");
                            break;
                        case DtoType.JoinChatRoom:
                            JoinChatRoomDto joinChatRoomDto = JsonSerializer.Deserialize<JoinChatRoomDto>(reciveJson);
                            _chatRooms.TryGetValue(joinChatRoomDto.chatRoomId, out ChatRoom joinChatRoom);
                            joinChatRoom.roomUsers.TryAdd(newUser.id, newUser);
                            await newUser.ListenMessage(joinChatRoomDto);
                            var joinRoomMsg = new RecivedMessageDto(joinChatRoomDto.chatRoomId, $"{newUser.name}加入聊天室!");
                            var joinBroadcastMessage = new BroadcastMessageDto("系統訊息", joinRoomMsg);
                            await BroadcastMessage(joinBroadcastMessage, joinChatRoom.roomUsers);
                            break;
                        case DtoType.LeaveChatRoom:
                            LeaveChatRoomDto leaveChatRoomDto = JsonSerializer.Deserialize<LeaveChatRoomDto>(reciveJson);
                            _chatRooms.TryGetValue(leaveChatRoomDto.chatRoomId, out ChatRoom leaveChatRoom);
                            await newUser.ListenMessage(leaveChatRoomDto);
                            var leaveRoomMsg = new RecivedMessageDto(leaveChatRoom.chatRoomId, $"{newUser.name}離開聊天室!");
                            var leaveBroadcastMessage = new BroadcastMessageDto("系統訊息", leaveRoomMsg);
                            await BroadcastMessage(leaveBroadcastMessage, leaveChatRoom.roomUsers);
                            leaveChatRoom.roomUsers.TryRemove(newUser.id, out newUser);
                            break;
                        case DtoType.RecivedMessage:
                            RecivedMessageDto message = JsonSerializer.Deserialize<RecivedMessageDto>(reciveJson);
                            _chatRooms.TryGetValue(message.chatRoomId, out var messageRoom);
                            var broadcastMessage = new BroadcastMessageDto(newUser.name, message);
                            await BroadcastMessage(broadcastMessage, messageRoom.roomUsers); // 将消息广播给其他所有客户端
                            break;
                        case DtoType.Error:
                            break;
                        case DtoType.PongDto:
                            newUser.lastPong = DateTime.Now;
                            Console.WriteLine($"{newUser.name}傳回Pong");
                            break;
                        case DtoType.ProgramClose:
                            Console.WriteLine("收到關閉訊號");
                            await newUser.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{newUser.name}異常停止心跳", e);
            }
            finally
            {
                newUser.heartbeatTimer.Stop();
                newUser.heartbeatTimer.Dispose();
                _programUsers.TryRemove(newUser.id, out newUser); newUser.webSocket.Abort();
                webSocket.Dispose();
                Console.WriteLine($"{newUser.name}已斷線");

            }
        }

        private async Task SendPingTo(ConnectedUser newUser)
        {
            if (newUser.webSocket.State == WebSocketState.Open)
            {
                PingDto pingDto = new PingDto();
                await newUser.ListenMessage(pingDto);
                Console.WriteLine($"向{newUser.name}發送Ping");
            }
        }
        private async Task CreateChatRoom()
        {
            ChatRoom chatRoom = new ChatRoom();
            _chatRooms.Add(chatRoom.chatRoomId, chatRoom);
            await BroadcastMessage(new CreateChatRoomDto(chatRoom.chatRoomId), _programUsers);//通知所有人房間成立了
        }
        private async Task CloseChatRoom() { }
        private async Task BroadcastMessage<T>(T masterDto, ConcurrentDictionary<string, ConnectedUser> conUsers) where T : MasterDto
        {
            var messageDtoJson = JsonSerializer.Serialize(masterDto);
            var buffer = Encoding.UTF8.GetBytes(messageDtoJson);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var pair in conUsers)
            {
                await pair.Value.webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
