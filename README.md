# Portfolio_AutoBulletSurvivor

![image](https://github.com/user-attachments/assets/a96ea900-9bdb-44f8-be4f-25e2d8974522)


단일 책임 원칙을 기반으로 시스템 설계
- 게임 내 주요 시스템을 효율적으로 관리를 목적으로 각 Manager는 단일 책임 원칙(SRP)에 기반하여 특정 역할에 할당.
  
GameManager를 중심으로 연계성 고려
- GameManager를 중심으로 상호 연결되어 전체 게임 흐름을 제어하도록 구성했습니다.


![image](https://github.com/user-attachments/assets/145fd240-49d6-49cb-bbf9-cfaf2eb41fe3)


Inventory 프로세스 구성
- 게임에서 Enemy를 처치하면서 생성된 아이템은 Raid_InventoryManager에 의해 관리
  
수집 및 탈출 흐름 기반의 쉘터 인벤토리 JSON 동기화 시스템
- 아이템 “수집 – 탈출 – 판매 및 강화” 을 통해 전반적인 콘텐츠 구성 및 Shelter(쉘터) 인벤토리로 JsonData를 갱신.

![image](https://github.com/user-attachments/assets/96fb3309-9c55-44e2-8f0e-66581de0ce6a)


Skill Manager / Skill 구현
- 각 스킬마다 Script를 통해 속성(properties) 부여
- SkillManager를 참조하여 하위 스킬 객체를 관리.
- SetActive(true/false)로 스킬의 활성화 유무를 파악
  
Random Skill Level 필터링 구현
-  스킬을 획득하여 최대 한도 달성 시 Skill Level 필터링을 통해 최대 레벨 시 -> HP회복용 선택지로 변환.



![image](https://github.com/user-attachments/assets/07d0e2a4-9455-4535-ba6d-4337baa04d3e)


Enemy 스크립트 구현- 코드 재사용성을 고려하여 하나의 스크립트로 구성
- 여러 형태의 Enemy Type을 구분
  
Enemy (Type/Properties) 타입에 따른 속성 
- Enemy Type에 맞게 필요한 속성(properties)부여
- Enemy 피격/처치 시 데미지 폰트 및 아이템 생성.

![image](https://github.com/user-attachments/assets/e5f85063-0d73-4273-9a64-53cd7c697cca)


Unity의 LitJson 활용
저장된 Json 데이터가 존재 하지 않을 경우 
- 처음 접속한 유저로 정의하여 새로운 저장 데이터 생성과 튜토리얼로 진행.
저장된 Json 데이터가 존재 할 경우
- loadJson을 통해 이전에 저장한 데이터를 전체적으로 호출.
NPC와의 판매 및 강화를 진행한 경우
- 상호작용 하는 아이템 데이터의 Inventory JSON 갱신
- JSON 저장 파트를 구별해 두어 저장이 필요한 프로세스에 적용.

![image](https://github.com/user-attachments/assets/2d0bd310-e48e-4e9d-8732-28038364c8c7)


Steam 도전과제
- 특정 레벨 달성시 Steamwork를 통해 도전과제 달성
- Steamwork 관리자 기능을 통해 도전과제 부여 및 설정.

