<template>
    <template v-if="connectState">
        <div class="chat_container">
            <div class="chat_icon_block">
                <div class="myHead">
                    <img src="../public/imgs/DjV5mx-rLjR.gif" alt="">
                </div>
                <div class="userName">{{userName}}</div>
                <div class="chat_icon">
                    <div v-for="room in livingRooms" class="myIcon" style="position :relative" @click="changeChatRoom(room)">
                        <div class="btn_block" @click="closeChatRoom(room)"><font-awesome-icon icon="xmark" class="btn floatRT" style="color:red;" /></div>
                        <span style=" white-space: nowrap;">{{room.chatRoomId}}</span>
                        <div>
                            <template v-if="room.isJoined">
                                <div><font-awesome-icon icon="user-secret" style="color: #63E6BE;" />{{room.users.length}}</div>
                                <div class="btn_block" @click="leaveChatRoom(room.chatRoomId)"><font-awesome-icon icon="xmark" class="btn" /></div>
                            </template>
                            <template v-else>
                                <div class="btn_block" @click="joinChatRoom(room.chatRoomId)"><font-awesome-icon icon="check" class="btn" /></div>
                            </template>
                        </div>
                    </div>
                </div>
                <div class="btn_block" @click="createChatRoom">
                    <font-awesome-icon icon="plus" class="btn" />
                </div>
            </div>
            <div id="myChatList" class="chat_list">
                <div class="chatListTag" v-for="user in currentRoom.users" :key="user.userId">
                    <div class="head"><img src="../public/imgs/DjV5mx-rLjR.gif" alt=""></div>
                    <div class="mytext">
                        <div class="name">{{user.userName}}</div>
                    </div>
                </div>
            </div>
            <div class="chat_box">
                <div id="chatRoom" class="chat_message" ref="chatContainer">
                    <div class="message_row other-message">
                        <div v-for="rowMessage in currentRoom.messages" :key="rowMessage.message" class="message-content">
                            <img class="head" src="../public/imgs/DjV5mx-rLjR.gif" alt="">
                            <label class="head">{{ rowMessage.massengerSenderName }}</label>
                            <div class="message-text">{{ rowMessage.message }}</div>
                            <div class="message-time">{{ rowMessage.messageTimestamp }}</div>
                        </div>
                    </div>
                </div>
                <div class="chat_input">
                    <input v-model="thisMessage.message" @keydown.enter="sendMessage" class="sendMsg" type="text"
                           placeholder="請輸入訊息">
                    <div class="myIcon2">
                        <i class="fa fa-share send_icon"></i>
                    </div>
                </div>
            </div>
            <div class="btn_block" @click="disconnect"><font-awesome-icon icon="xmark" class="btn floatRT" style="color:red;" /></div>
        </div>
    </template>
    <template v-else>
        <div>
            <div class="login">
                <input type="text" v-model="userName" placeholder="Username">
                <!--<input type="password" placeholder="Password">-->
                <button @click="createConnect">Login</button>
            </div>
        </div>
    </template>
</template>
<script>
    import('../../chatroom/public/chatRoom.css');
    import('../../chatroom/public/Login.css');
    import { API_URL } from '../src/urlPathRouter.js';
    export default {
        data() {
            return {
                connectState: false,
                images: [`DjV5mx-qma7.gif`, `DjV5mx-09NQ.gif`, `DjV5mx-rLjR.gif`],
                headerAddress: `DjV5mx-qma7.gif`,
                userName: null,
                wsUrl: `ws://${API_URL}/ws`,
                program_ws: null,
                thisMessage: { dtoType: 3, chatRoomId: null, message: null },
                livingRooms: [],
                currentRoom: { messages: [] },
                pongDto: { dtoType: 52 },
                createChatRoomDto: { dtoType: 1 },
                joinChatRoomDto: { dtoType: 2, chatRoomId: null },
                leaveChatRoomDto: { dtoType: 4, chatRoomId: null },
                closeChatRoomDto: { dtoType: 7, chatRoomId: null },
            }
        },
        mounted() {

        },
        methods: {
            async createConnect() {
                try {
                    var app = this;
                    if (app.userName === null) {
                        console.log('請輸入名稱');
                        return;
                    }
                    var connectString = `${app.wsUrl}/newConnecting?userName=${app.userName}`;
                    app.program_ws = await new WebSocket(connectString);
                    app.program_ws.onopen = function (event) {
                        app.connectState = true;
                        console.log("連線成功", event);
                    };
                    app.program_ws.onmessage = function (event) {
                        var reciveDto = JSON.parse(event.data);
                        switch (reciveDto.dtoType) {
                            case 0:
                                break;
                            case 1:
                                app.livingRooms.push(reciveDto);
                                break;
                            case 2:
                                var joiningRoom = app.livingRooms.find(r => r.chatRoomId === reciveDto.chatRoomId);
                                joiningRoom.isJoined = true;
                                joiningRoom.messages = [];
                                break;
                            case 5:
                                var messageRoom = app.livingRooms.find(r => r.chatRoomId === reciveDto.chatRoomId);
                                messageRoom.messages.push(reciveDto);
                                app.scrollToBottom();
                                break;
                            case 4:
                                var leavingRoom = app.livingRooms.find(r => r.chatRoomId === reciveDto.chatRoomId);
                                leavingRoom.isJoined = false;
                                break;
                            case 6:
                                var findRoom = app.livingRooms.find(r => r.chatRoomId === reciveDto.chatRoomId);
                                findRoom.users = reciveDto.users;
                                break;
                            case 7:
                                //var findRoom = app.livingRooms.find(r => r.chatRoomId === reciveDto.chatRoomId);
                                var index = app.livingRooms.findIndex(r => r.chatRoomId === reciveDto.chatRoomId);
                                app.livingRooms.splice(index, 1);
                                alert(`聊天室${findRoom.chatRoomId}被關閉`);
                                break;
                            case 51:
                                console.log('收到ping');
                                app.program_ws.send(JSON.stringify(app.pongDto));
                                break;
                            default:
                        }
                    };
                    app.program_ws.onclose = function (event) {
                        console.log('連線關閉', event);
                        app.connectState = false;
                    };
                    app.program_ws.onerror = function (error) {
                        alert('WebSocket error: ', error);
                    };
                } catch (e) {
                    console.log("連線開啟失敗!!");
                }
            },
            scrollToBottom() {
                this.$nextTick(() => {
                    const chatContainer = this.$refs.chatContainer;
                    if (chatContainer) {
                        chatContainer.scrollTop = chatContainer.scrollHeight;
                    }
                });
            },
            sendMessage() {
                var app = this;
                if (app.program_ws.readyState === WebSocket.OPEN) {
                    //app.thisMessage.messageTimestamp = (new Date()).toISOString();
                    app.program_ws.send(JSON.stringify(app.thisMessage));
                    app.thisMessage.message = null;
                } else {
                    console.error('WebSocket 未連接。');
                }
            },
            disconnect() {
                var app = this;
                try {
                    app.program_ws.close();
                } catch (e) {
                    alert("關閉失敗");
                }
            },
            createChatRoom() {
                var app = this;
                app.program_ws.send(JSON.stringify(app.createChatRoomDto))
            },
            joinChatRoom(selectedRoomId) {
                try {
                    var app = this;
                    app.joinChatRoomDto.chatRoomId = selectedRoomId;
                    app.program_ws.send(JSON.stringify(app.joinChatRoomDto));
                } catch (e) {
                    alert("加入聊天室失敗");
                }
            },
            leaveChatRoom(selectedRoomId) {
                try {
                    var app = this;
                    app.leaveChatRoomDto.chatRoomId = selectedRoomId;
                    app.program_ws.send(JSON.stringify(app.leaveChatRoomDto));
                } catch (e) {
                    alert("離開聊天室失敗");
                }
            },
            changeChatRoom(room) {
                var app = this;
                app.currentRoom = room;
                app.thisMessage.chatRoomId = room.chatRoomId;
            },
            closeChatRoom(room) {
                try {
                    var app = this;
                    app.closeChatRoomDto.chatRoomId = room.chatRoomId;
                    app.program_ws.send(JSON.stringify(app.closeChatRoomDto));
                } catch (e) {
                    alert("關閉聊天室失敗");
                }
            },
        },
    }
</script>