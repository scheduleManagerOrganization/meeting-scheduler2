응소실 10조 조별 프로젝트 데이터베이스 및 API라우터 레포지토리 입니다.

📋 AI 미팅 스케줄러 API 명세서
Base URL
https://meeting-scheduler-csharp-1zpz.onrender.com

🔑 Authentication
JWT Bearer Token을 사용합니다.

Authorization: Bearer {token}
📚 Endpoint Summary
#	Method	Endpoint	Description
1	GET	/	서버 상태 확인
2	GET	/health	헬스 체크 (MongoDB 연결 포함)
3	GET	/api/debug/routes	서버 라우트 디버그 조회
4	POST	/api/init-db	테스트 데이터 초기화 (alice, bob)
5	POST	/api/auth/register	회원가입
6	POST	/api/auth/login	로그인
7	POST	/api/teams	팀 생성
8	POST	/api/teams/join	팀 참여 (초대코드)
9	GET	/api/teams/{userId}	내 팀 목록
10	POST	/api/availability	가능 시간 저장 (upsert)
11	GET	/api/availability/{userId}/{date}	개인 일정 조회
12	GET	/api/availability/team/{teamId}/{date}	팀 일정 조회
13	POST	/api/meetings	미팅 생성
14	GET	/api/meetings/{meetingId}	미팅 상세
15	GET	/api/meetings/team/{teamId}	팀 미팅 목록
16	POST	/api/suggest-slots	규칙 기반 슬롯 추천
17	POST	/api/ai-recommend-slots	AI 기반 슬롯 추천
18	POST	/api/ai/recommend-slots	AI 추천 alias
19	POST	/api/ai-recommend-slot	AI 추천 alias
20	GET	/api/slots/{meetingId}	추천 슬롯 조회
21	POST	/api/respond-slot	슬롯 응답 저장
22	WS	/meetingHub	SignalR Hub
1) GET /
서버 상태 확인

Response 200
{
  "message": "🗓️ AI 미팅 스케줄러 API",
  "status": "running",
  "websocket": "supported",
  "timestamp": "2026-05-01T17:32:06.43961Z"
}


2) GET /health
헬스 체크

Response 200
{
  "status": "healthy",
  "mongodb": "connected",
  "timestamp": "2026-05-01T17:32:06.43961Z"
}

3) GET /api/debug/routes
현재 서버에 등록된 라우트 목록 조회 (디버그용)

Response 200
{
  "success": true,
  "count": 23,
  "data": [
    { "route": "/health", "order": 0 },
    { "route": "api/ai-recommend-slots", "order": 0 }
  ],
  "timestamp": "2026-05-03T01:23:45.7215097Z"
}

4) POST /api/init-db
테스트 계정 생성 (이미 존재하면 skip)

Response 200
{
  "success": true,
  "message": "Database initialized",
  "data": {
    "users": ["alice@example.com", "bob@example.com"]
  }
}


5) POST /api/auth/register
Request
{
  "email": "user@example.com",
  "password": "password123",
  "name": "사용자 이름",
  "timezone": "Asia/Seoul"
}
Response 200
{
  "success": true,
  "data": {
    "user_id": "69f4a6b187290fb447c0b3af",
    "name": "사용자 이름",
    "email": "user@example.com",
    "token": "eyJhbGciOiJIUzI1NiIs..."
  }
}


6) POST /api/auth/login
Request
{
  "email": "alice@example.com",
  "password": "alice1234"
}
Response 200
{
  "success": true,
  "data": {
    "user_id": "69f4a6b187290fb447c0b3af",
    "name": "Alice Kim",
    "email": "alice@example.com",
    "token": "eyJhbGciOiJIUzI1NiIs..."
  }
}


7) POST /api/teams
Request
{
  "team_name": "테스트팀",
  "owner_id": "69f4a6b187290fb447c0b3af",
  "description": "API 테스트용 팀입니다"
}
Response 200
{
  "success": true,
  "data": {
    "team_id": "69f4e39d892000f58058f62e",
    "join_code": "2FBTTPQT"
  }
}


8) POST /api/teams/join
Request
{
  "join_code": "2FBTTPQT",
  "user_id": "69f4a6b287290fb447c0b3b0"
}
Response 200
{
  "success": true,
  "data": {
    "team_id": "69f4e39d892000f58058f62e",
    "team_name": "테스트팀"
  }
}


9) GET /api/teams/{userId}
Response 200
{
  "success": true,
  "data": [
    {
      "team_id": "69f4e39d892000f58058f62e",
      "team_name": "테스트팀",
      "join_code": "2FBTTPQT",
      "member_count": 2
    }
  ]
}


10) POST /api/availability
같은 user_id + date는 upsert 됩니다.

Request
{
  "user_id": "69f4a6b187290fb447c0b3af",
  "date": "2026-05-15",
  "team_id": "69f4e39d892000f58058f62e",
  "slots": [
    { "start": "10:00", "end": "12:00" },
    { "start": "14:00", "end": "16:00" }
  ]
}
Response 200
{
  "success": true,
  "message": "일정이 저장되었습니다"
}


11) GET /api/availability/{userId}/{date}
Response 200
{
  "success": true,
  "data": {
    "id": "69f4e13426c17cbb1fc2daf9",
    "userId": "69f4a6b187290fb447c0b3af",
    "date": "2026-05-15",
    "slots": [
      { "start": "10:00", "end": "12:00" },
      { "start": "14:00", "end": "16:00" }
    ],
    "updatedAt": "2026-05-01T17:32:15.23Z"
  }
}


12) GET /api/availability/team/{teamId}/{date}
Response 200
{
  "success": true,
  "data": [
    {
      "user_id": "69f4a6b187290fb447c0b3af",
      "user_name": "Alice Kim",
      "slots": [
        { "start": "10:00", "end": "12:00" },
        { "start": "14:00", "end": "16:00" }
      ]
    },
    {
      "user_id": "69f4a6b287290fb447c0b3b0",
      "user_name": "Bob Park",
      "slots": [
        { "start": "10:00", "end": "13:00" },
        { "start": "15:00", "end": "18:00" }
      ]
    }
  ]
}


13) POST /api/meetings
Request
{
  "team_id": "69f4e39d892000f58058f62e",
  "title": "주간 회의",
  "description": "주간 진행 상황 점검",
  "duration_minutes": 60,
  "creator_id": "69f4a6b187290fb447c0b3af",
  "deadline_date": "2026-05-20"
}
Response 200
{
  "success": true,
  "data": {
    "meeting_id": "69f4e39f892000f58058f648"
  }
}


14) GET /api/meetings/{meetingId}
Response 200
{
  "success": true,
  "data": {
    "id": "69f4e39f892000f58058f648",
    "teamId": "69f4e39d892000f58058f62e",
    "title": "주간 회의",
    "description": "주간 진행 상황 점검",
    "durationMinutes": 60,
    "creatorId": "69f4a6b187290fb447c0b3af",
    "status": "proposing",
    "deadlineDate": "2026-05-20",
    "createdAt": "2026-05-01T17:32:15.23Z",
    "finalizedSlotId": null
  }
}


15) GET /api/meetings/team/{teamId}
Response 200
{
  "success": true,
  "data": [
    {
      "id": "69f4e39f892000f58058f648",
      "teamId": "69f4e39d892000f58058f62e",
      "title": "주간 회의",
      "description": "주간 진행 상황 점검",
      "durationMinutes": 60,
      "creatorId": "69f4a6b187290fb447c0b3af",
      "status": "proposing",
      "deadlineDate": "2026-05-20",
      "createdAt": "2026-05-01T17:32:15.23Z",
      "finalizedSlotId": null
    }
  ]
}


16) POST /api/suggest-slots (규칙 기반 추천)
Request
{
  "meeting_id": "69f4e39f892000f58058f648"
}
Response 200
{
  "success": true,
  "data": {
    "suggested_count": 3
  }
}
추천 기준: UTC 오늘부터 7일, 각 날짜의 10시/14시/16시.



17~19) POST AI 추천 (동일 동작, 경로 alias)
/api/ai-recommend-slots

/api/ai/recommend-slots

/api/ai-recommend-slot

Request
{
  "meeting_id": "69f4e39f892000f58058f648"
}
Response 200 (추천 있음)
{
  "success": true,
  "data": {
    "recommended_count": 2,
    "slots": [
      {
        "id": "69f4e39f892000f58058f649",
        "meetingId": "69f4e39f892000f58058f648",
        "startTime": "2026-05-03T10:00:00",
        "endTime": "2026-05-03T11:00:00",
        "aiScore": 95.0,
        "aiReason": "팀원 가용시간이 가장 많이 겹칩니다.",
        "isFinalized": false,
        "responses": [],
        "createdAt": "2026-05-01T17:32:18.23Z"
      }
    ]
  }
}
Response 200 (추천 없음)
{
  "success": true,
  "data": {
    "recommended_slots": [],
    "message": "AI 추천 결과가 없습니다"
  }
}


20) GET /api/slots/{meetingId}
Response 200
{
  "success": true,
  "data": [
    {
      "id": "69f4e39f892000f58058f649",
      "meetingId": "69f4e39f892000f58058f648",
      "startTime": "2026-05-03T10:00:00",
      "endTime": "2026-05-03T11:00:00",
      "aiScore": 95.0,
      "aiReason": "팀원 가용시간이 가장 많이 겹칩니다.",
      "isFinalized": false,
      "responses": [],
      "createdAt": "2026-05-01T17:32:18.23Z"
    }
  ]
}


21) POST /api/respond-slot
Request
{
  "slot_id": "69f4e39f892000f58058f649",
  "user_id": "69f4a6b187290fb447c0b3af",
  "response": "yes"
}
Response 200
{
  "success": true,
  "message": "응답이 저장되었습니다"
}
🔌 SignalR (WebSocket)
Hub URL: /meetingHub

Client → Server
JoinTeamRoom(teamId)

JoinMeetingRoom(meetingId)

SendMessage(room, message, userName)

NotifyAvailabilityUpdated(userId, date, teamId)

NotifyMeetingCreated(meetingId, title, teamId)

NotifySlotResponseUpdated(slotId, userId, response)

Server → Client
joined

receive_message

availability_updated

meeting_created

slot_response_updated

⚠️ Common Errors
Code	Meaning
400	잘못된 요청
401	인증 실패
404	리소스 없음
500	서버 내부 오류
예시:

{
  "success": false,
  "error": "에러 메시지"
}
🔑 ID Format
MongoDB ObjectId (24자리 hex 문자열)
예: 69f4a6b187290fb447c0b3af

🧪 Test Accounts
POST /api/init-db 호출 시 자동 생성됩니다.

Alice Kim / alice@example.com / alice1234

Bob Park / bob@example.com / bob1234
