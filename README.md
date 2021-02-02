# 더 폰 (The Pawn)

![그림1](https://user-images.githubusercontent.com/55977034/106613770-e7583f80-65ad-11eb-9e2b-c2b303a630ef.png)

- 안드로이드 환경에서 작동하는 로그라이트 3D 모바일 RPG
- Playstore: https://play.google.com/store/apps/details?id=com.SadPartners.ThePawn

##  요약
---
> 마을씬에선 상점UI나 NPC 캐릭터와 상호작용하여 퀘스트 시스템 사용 가능.

> 마을씬에서 포털을 통해 던전에 입장하려고 하면 카드 선택 시스템을 통해 던전의 특정 구역에서 사용할 패시브 사용
 
> 던전씬에선 절차적 생성된 랜덤 던전을 탐험하며 다양한 공격패턴을 지닌 몬스터들을 사냥한다.


## 개발 환경
---
- Unity 2019.4.16f1 (LTS)
- Visual Studio 2017

## 설치방법
---
- 위의 플레이스토어 링크를 통해 안드로이드 모바일 환경에 설치하여 플레이.
- 본 리포지토리를 Fork하여 위와 동일한 개발 환경 상에서 구동.

## 사용법
---

- 시작 및 로딩화면

![그림2](https://user-images.githubusercontent.com/55977034/106613772-e7f0d600-65ad-11eb-8a10-7c591d668a22.png)

![그림3](https://user-images.githubusercontent.com/55977034/106613775-e8896c80-65ad-11eb-857d-3232cb0c1b14.png)

- 마을 맵 내에선 돌아다니며 다양한 NPC들과 상호작용을 통해 퀘스트를 획득할 수 있으며, 셰이더 그래프로 작성한 커스텀 셰이더를 통해 캐릭터가 벽 뒤에 위치하더라도 가시성을 확보했다.

![그림4](https://user-images.githubusercontent.com/55977034/106613778-e9220300-65ad-11eb-8437-0f6d9ed2df1f.png)

![그림5](https://user-images.githubusercontent.com/55977034/106613782-e9ba9980-65ad-11eb-9261-1e2290409995.png)

- 퀘스트 및 대화 시스템을 진행하여 퀘스트를 확보할 수 있으며, 옵션 버튼을 통해 사운드 및 데이터 초기화 등의 설정을 수정할 수 있다.

![그림8](https://user-images.githubusercontent.com/55977034/106613754-e45d4f00-65ad-11eb-8d9a-771c0c2431f5.png)

![그림9](https://user-images.githubusercontent.com/55977034/106613761-e6271280-65ad-11eb-977d-55d0fe676d65.png)
  
- 마을 내에서 이용 가능한 상점 UI를 통해 다양한 재화나 상품을 구매하거나, 인벤토리를 통해 아이템을 착용 및 해제할 수 있다.

![그림6](https://user-images.githubusercontent.com/55977034/106613785-ea533000-65ad-11eb-9075-7747587ed180.png)

![그림7](https://user-images.githubusercontent.com/55977034/106613983-29818100-65ae-11eb-8339-29979726ce96.png)
  
- 마을에서 던전으로 입장하게 되면 절차적 생성 알고리즘을 이용하여 랜덤 던전이 생성되고, 스테이지별로 다양한 몬스터들을 상대로 플레이어 캐릭터를 조이스틱 및 버튼을 통해 조작하며 로그라이트 풍 전투 시스템을 즐길 수 있다.

![그림10](https://user-images.githubusercontent.com/55977034/106613762-e6271280-65ad-11eb-9bb2-5c74fd947cbf.png)

![그림11](https://user-images.githubusercontent.com/55977034/106613766-e6bfa900-65ad-11eb-83bd-a756c9c324db.png)
