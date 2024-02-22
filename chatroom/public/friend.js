const web_Me = {
    "name": "Johnny",
    "title": "�����e��",
    "email": "nintendof1@gmail.com",
    "photo": "img/me/Johnny.png",
    "photo2": "img/me/Johnny2.jpg"
}

const web_Friends = [
    {
        "name": "Alpha_Team",
        "title": "Alpha ���չζ�",
        "email": "Alpha_Team@gmail.com",
        "photo": "img/friends/Alpha_Team.png",
        "messageNum": 3
    },
    {
        "name": "Beta_Team",
        "title": "Beta ���չζ�",
        "email": "Beta_Team@gmail.com",
        "photo": "img/friends/Beta_Team.png",
        "messageNum": 2
    },
    {
        "name": "Big Boss",
        "title": "���q�Ѥj",
        "email": "BigBoss@gmail.com",
        "photo": "img/friends/BigBoss.png",
        "messageNum": 0
    },
    {
        "name": "David",
        "title": "�M�׸g�z",
        "email": "David@gmail.com",
        "photo": "img/friends/David.png",
        "messageNum": 7
    },
    {
        "name": "Final_Check",
        "title": "�̫��˴�",
        "email": "Final_Check@gmail.com",
        "photo": "img/friends/Final_Check.png",
        "messageNum": 0
    },
    {
        "name": "Grace",
        "title": "ͺ��",
        "email": "Grace@gmail.com",
        "photo": "img/friends/Grace.png",
        "messageNum": 0
    },
    {
        "name": "Judy",
        "title": "���s���u",
        "email": "Judy@gmail.com",
        "photo": "img/friends/Judy.png",
        "messageNum": 0
    },
    {
        "name": "LittleBoss",
        "title": "�G����",
        "email": "LittleBoss@gmail.com",
        "photo": "img/friends/LittleBoss.jpg",
        "messageNum": 0
    },
    {
        "name": "Peter",
        "title": "�����o",
        "email": "Peter@gmail.com",
        "photo": "img/friends/Peter.png",
        "messageNum": 6
    },
    {
        "name": "Steve",
        "title": "�������",
        "email": "Steve@gmail.com",
        "photo": "img/friends/Steve.png",
        "messageNum": 14
    },
    {
        "name": "Susan",
        "title": "�A���`��",
        "email": "Susan@gmail.com",
        "photo": "img/friends/Susan.png",
        "messageNum": 8
    },
    {
        "name": "Tech_boy",
        "title": "�޳N�ȪA",
        "email": "Tech_boy@gmail.com",
        "photo": "img/friends/Tech_boy.png",
        "messageNum": 0
    },
    {
        "name": "Tony",
        "title": "�޳N�ȪA",
        "email": "Tony@gmail.com",
        "photo": "img/friends/Tony.png",
        "messageNum": 1
    }
]

let friendCard = '';

web_Friends.map(data => {
    if (data.messageNum > 0) {
        if (data.name == 'David') {
            friendCard += `
        <div class="chatListTag active">
          <div class="head"><img src="${data.photo}" alt=""></div>
          <div class="mytext">
            <div class="name">${data.name}</div>
            <div class="dec">${data.title}</div>
          </div>
          <div class="msg_num">${data.messageNum}</div>
        </div>
      `
        } else {
            friendCard += `
        <div class="chatListTag">
          <div class="head"><img src="${data.photo}" alt=""></div>
          <div class="mytext">
            <div class="name">${data.name}</div>
            <div class="dec">${data.title}</div>
          </div>
          <div class="msg_num">${data.messageNum}</div>
        </div>
      `
        }
    } else {
        friendCard += `
      <div class="chatListTag">
        <div class="head"><img src="${data.photo}" alt=""></div>
        <div class="mytext">
          <div class="name">${data.name}</div>
          <div class="dec">${data.title}</div>
        </div>
      </div>
    `
    }
})
document.getElementById('myChatList').innerHTML = friendCard;