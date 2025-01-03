- ## ClientSide
	- 加入 /Models/EFModels
	- 加入 Models/Infra/HashUtility.cs (用來做密碼雜湊的公用函式)
	- 加入 /Controllers/MembersController.cs 
	- 在Web.config中加入 appSetting , key="salt"
	- 註冊新會員 功能
	- 


- ## ServerSide
	- 加入 /Models/EFModels
	- 加入 MembersController 和 /Views/Members
	- 加入 Models/Infra/HashUtility.cs (用來做密碼雜湊的公用函式)
	- 