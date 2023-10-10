# Block It
“Block It”은 모바일 플랫폼 기반 [쿼리도 보드 게임](https://namu.wiki/w/%EC%BF%BC%EB%A6%AC%EB%8F%84)입니다. 
인공지능과의 대결을 통해 실력을 키우고 친구들과 대전을 해보세요.

https://github.com/SeoulTechTCPGame/block-it/assets/88719152/72cbf049-8967-4ab0-9965-6423c16f92c8

안드로이드 혹은 IOS에서 다운받아 더욱 편하게 즐길 수 있습니다.

[다운로드 링크]()

## 목차
- [플레이 방식](https://github.com/SeoulTechTCPGame/block-it/edit/refactor-OhJunnie/README.md#%ED%94%8C%EB%A0%88%EC%9D%B4-%EB%B0%A9%EC%8B%9D)
- [설명](https://github.com/SeoulTechTCPGame/block-it/edit/refactor-OhJunnie/README.md#%EC%84%A4%EB%AA%85)
- [개발진 및 담당](https://github.com/SeoulTechTCPGame/block-it/edit/refactor-OhJunnie/README.md#%EA%B0%9C%EB%B0%9C%EC%A7%84-%EB%B0%8F-%EB%8B%B4%EB%8B%B9)
- [개발 일정](https://github.com/SeoulTechTCPGame/block-it/edit/refactor-OhJunnie/README.md#date)

## 플레이 방식
- [](로딩)
  - 해당 화면에서 로그인을 할 수 있습니다.
  - 회원 가입을 하여 새로운 계정을 만들 수 있습니다.
  - 로그인이 필요없는 게스트 모드를 이용해 보세요.
  - 만약 이미 로그인이 되어 있다면 자동으로 홈으로 이동합니다.
- [](홈)  

<img width="242" alt="스크린샷 2023-09-17 오후 3 43 35" src="https://github.com/SeoulTechTCPGame/block-it/assets/88719152/16f0ed69-b14d-4857-97ba-f6a87c41f550">. 
  
  - 옵션, 프로필, 원하는 게임 모드 선택 등으로 이동이 가능합니다.
  
- [](프로필)
  - 현재 계정의 정보를 볼 수 있습니다.
  - 계정 정보 변경를 변경할 수 있습니다.(프로필 이미지, 비밀번호 변경 등)
- [](옵션)  
<img width="241" alt="스크린샷 2023-09-17 오후 3 45 38" src="https://github.com/SeoulTechTCPGame/block-it/assets/88719152/43e248e3-a4a5-4dc0-997c-d696fa51cff6">    
    

  - BGM 혹은 사운드 이펙트를 조절할 수 있습니다.
  - 진동을 on/off할 수 있습니다.
  - 언어는 한국어와 영어만 지원합니다.
  - 해당 기능들은 게임을 꺼도 저장이 됩니다.
  - 튜토리얼을 살펴볼 수 있습니다.
- [](튜토리얼)  
<img width="240" alt="스크린샷 2023-09-17 오후 3 46 45" src="https://github.com/SeoulTechTCPGame/block-it/assets/88719152/80eecd6f-539d-467b-a160-fd57671f6dba">


  - 게임 플레이 방법이 서술되어 있습니다.

- [](플레이)
  - 모드에 맞게 게임을 플레이할 수 있습니다.
  - 로컬 모드: 하나의 기기로 둘이서 플레이할 수 있습니다.
  - AI 모드: 혼자서 AI와 대전을 할 수 있습니다.
  - Wifi모드: 같은 Wifi 내에 있는 또 다른 유저와 대전을 할 수 있습니다.
  - 플레이 이후 어떻게 플레이 했는 지 복기할 수 있습니다.

## 설명
- 의도 및 동기: 
- 특징
  - 기획 과정은 [Figma](https://www.figma.com/file/axcqNlMNnMknrpX1aesM97/Quoridor_English_Version?type=design&node-id=0-1&mode=design) 혹은 깃허브의 [Project](https://github.com/orgs/SeoulTechTCPGame/projects/9)에서 확인할 수 있습니다.
  - Firebase를 통해 회원 가입 및 로그인, DB를 관리합니다.
  - 유저 정보를 MsSQL 에서 관리합니다.
  - Wifi 모드는 Unity Mirror를 통해 구현되었습니다.
  - 언어 번역은 [구글 스프라이트 시트](https://docs.google.com/spreadsheets/d/1Dsj19n_rK5MEaxfu_4dn2NMJHAj3pcd4A-eY-7MjJ2M/edit#gid=0)를 통해 번역되었습니다.
  - AI는 Monte Carlo 방식을 따르며 Python으로 제작되었습니다.

- 클래스 다이어그램 or FSM
  - [씬 이동(FSM)]()
  - [핵심 로직 (클래스 다이어그램)]()
  - [AI]()

## 개발진 및 담당
- 전효정([junnie082](https://github.com/junnie082)): 팀장, 게임 로직, 기획
- 오성혁([seong0929](https://github.com/seong0929)): 부팀장, 기획, 개발
- 김다은([KimDa99](https://github.com/KimDa99)): 게임 로직, UI 디자인
- 이상욱([az0t0](https://github.com/az0t0)): 서버 통신, DB
- 이형진([Lee-Hyeong-Jin](https://github.com/Lee-Hyeong-Jin)): AI, AI 서버

### 사용한 툴
- Unity(C#)
- Unity Mirror
- GitHub
- Source Tree
- Firebase
- Figma
- Visual Studio & Visual Studio Code
- Python
- JavaScript
- Express

##  Date
- 23.4.? ~ 23.5.11 기획
- 23.5.12 ~ 23.10.11 개발
- 23.10.11 ~ 23.10.18 QA 및 최적화 및 버그 수정 및 개선
