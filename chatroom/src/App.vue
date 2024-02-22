<template>
    <div class="chat_container">
        <div class="chat_icon_block">
            <div class="myHead">
                <img src="img/me/Johnny.png" alt="">
            </div>
            <div class="chat_icon">
                <div class="myIcon active">
                    <i class="fa fa-comment msg_Y"></i>
                </div>
                <div class="myIcon">
                    <i class="fa fa-user msg_N"></i>
                </div>
                <div class="myIcon">
                    <i class="fa fa-users msg_N"></i>
                </div>
                <div class="myIcon">
                    <i class="fa fa-envelope msg_N"></i>
                </div>
                <div class="myIcon">
                    <i class="fa fa-cog msg_N"></i>
                </div>
                <div class="myIcon">
                    <i class="fa fa-sign-out msg_N"></i>
                </div>
            </div>
        </div>
        <div id="myChatList" class="chat_list">
            <div class="chatListTag" v-for="room in livingRooms" :key="room.chatRoomId" @click="changeChatRoom(room)">
                <div class="head"><img src="img/friends/Alpha_Team.png" alt=""></div>
                <div class="mytext">
                    <div class="name">{{room.chatRoomId}}</div>
                    <div class="dec">{{room.chatRoomId}}</div>
                </div>
                <div class="msg_num">3</div>
                <button @click="joinChatRoom(room.chatRoomId)">加入</button>
                <button @click="leaveChatRoom(room.chatRoomId)">離開</button>
            </div>
            <button @click="createChatRoom">開新聊天室</button>
        </div>
        <div class="chat_box">
            <div id="chatRoom" class="chat_message">
                <div class="message_row other-message">
                    <div v-for="rowMessage in currentRoom.messages" :key="rowMessage.message" class="message-content">
                        <!--<img class="head" src="img/friends/David.png" alt="">-->
                        <label class="head">{{ rowMessage.massengerSenderName }}</label>
                        <div class="message-text">{{ rowMessage.message }}</div>
                        <div class="message-time">{{ rowMessage.messageTimestamp }}</div>
                    </div>
                </div>
            </div>
            <div class="chat_input">
                <div class="myIcon2 iconTag">
                    <i class="fa fa-smile-o ipt_icon"></i>
                    <div class="icon_block">
                        <div class="emoji_icon">
                            <img src="img/emoji/cry.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/cry2.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/I_dont_know.jpg" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/like.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/like2.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/omg.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/robot-face.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/smile.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/strange.png" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/what.jpg" alt="">
                        </div>
                        <div class="emoji_icon">
                            <img src="img/emoji/wow.png" alt="">
                        </div>
                    </div>
                </div>
                <div class="myIcon2">
                    <img src="img/annex.png" alt="">
                </div>
                <input v-model="thisMessage.message" @keydown.enter="sendMessage" class="sendMsg" type="text"
                       placeholder="請輸入訊息">
                <div class="myIcon2">
                    <i class="fa fa-share send_icon"></i>
                </div>
            </div>
        </div>
        <div class="me_block">
            <label style="color:black">{{ program_ws !== null ? program_ws.readyState : 2 }}</label>
            <input v-model="userName" type="text" />
            <button @click="createConnect">連線系統</button>
            <button @click="disconnect">斷開系統</button>
        </div>
    </div>
</template>
<script>
    import('../../chatroom/public/chatRoom.css');
    export default {
        data() {
            return {
                userName: null,
                wsUrl: 'ws://localhost:5000/ws',
                program_ws: null,
                thisMessage: {dtoType: 3, chatRoomId: null, message: null},
                livingRooms: [],
                joinedRooms: [],
                currentRoom: { messages: [] },
                pongDto: { dtoType: 52 },
                createChatRoomDto: { dtoType: 1 },
                joinChatRoomDto: { dtoType: 2, chatRoomId: null },
                leaveChatRoomDto: { dtoType: 4, chatRoomId: null },
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
                        console.log("連線成功",event);
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
                                app.thisMessage.chatRoomId = reciveDto.chatRoomId;
                                joiningRoom.messages = [];
                                app.joinedRooms.push(joiningRoom);
                                break;
                            case 5:
                                joiningRoom.messages.push(reciveDto);
                                break;
                            case 4:
                                var leavingRoom = app.joinedRooms.find(r => r.chatRoomId === reciveDto.chatRoomId);
                                app.thisMessage.chatRoomId = null;
                                var index = app.joinedRooms.indexOf(leavingRoom);
                                app.joinedRooms.splice(index, 1);
                                break;
                            case 51:
                                console.log('收到ping');
                                app.program_ws.send(JSON.stringify(app.pongDto));
                                break;
                            default:
                        }
                    };
                    app.program_ws.onclose = function (event) {
                        console.log('連線關閉',event);
                    };
                    app.program_ws.onerror = function (error) {
                        alert('WebSocket error: ', error);
                    };
                } catch (e) {
                    console.log("連線開啟失敗!!");
                }
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
        },
    }
</script>