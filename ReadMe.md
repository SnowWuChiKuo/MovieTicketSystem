- ## ClientSide
    ### 資料庫
	- 加入 /Models/EFModels
	### 會員系統
	- 加入 註冊新會員
		- 加入 /Controllers/MembersController.cs 
		- 加入 /Models/Infra/HashUtility class
		- 在 Web.config 中加入 appSetting , key="salt"
		- 加入 /Models/ViewModels/RegisterVm class
		- 加入 RegisterConfirm View Page
	- 實作會員確認功能(未做發送email信驗證)
		- 加入 /Members/ActiveRegister, url是 /Members/ActiveRegister?memberId=&confirmCode=
		- 加入 ActiveRegister view page , 範本: empty
	- 實作登入/登出功能
		- 只有帳密正確且 IsConfirmed = true ,IsDeleted = false , IsBlackList = false 的會員才被允許登入，請事先準備已/未開通的會員各一，方便測式
	    - 修改 web.config，加入表單認證(cookie) => 
			 ``` 
			<authentication mode="Forms">
			<forms name="mySite" loginUrl="~/Members/Login" defaultUrl="~/Members/Index/" />
			</authentication>
		- 加入 /Models/ViewModels/LoginVm class
		- 修改 MembersController , add Login action
		- 加入 Login view page, 範本: create
		- 暫時做一個簡單的會員中心頁 /Members/Index
		- 修改 MemberControler, add Logout action
		- 修改 _Layout.csthml, add登入/登出的連結(沒登入與已登入，要顯示不同連結)
	- 實作修改個資
		- 在 /Members/Index(會員中心) 加入修改個資的連結
		- 加入 /Models/ViewModels/ProfileVm class
		- 加入 /Members/Profile action
		- 加入 Profile view page, 範本: Edit 		

- ## ServerSide
   ### 資料庫  
	- 加入 /Models/EFModels

   ### 後台模板
	- 修改 _Layout.cshtml, 加入後台模板的模板
	- 新增 模板到 wwwroot/css, wwwroot/js, /img
	- 新增 測試程式到 /Views/Movies/List.cshtml 當測試面板進行修改。
	- 修改 /Views/Home/Index.cshtml, 加入後台模板的模板
	
   ### 會員系統
	- 加入 MembersController 和 /Views/Members
	- 加入 Models/Infra/HashUtility.cs (用來做密碼雜湊的公用函式)

	
