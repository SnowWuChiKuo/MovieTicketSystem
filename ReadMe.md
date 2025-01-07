- ## ClientSide
    ### 資料庫
	- 加入 /Models/EFModels
	### 會員系統
	-實作 註冊新會員功能
		- 加入 /Controllers/MembersController.cs 
		- 加入 /Models/Infra/HashUtility class
		- 在 Web.config 中加入 appSetting , key="salt"
		- 加入 /Models/ViewModels/RegisterVm class
		- 加入 RegisterConfirm View Page
	- 實作會員確認功能   (未做發送email信驗證)
		- 加入 /Members/ActiveRegister, url是 /Members/ActiveRegister?memberId=&confirmCode=
		- 加入 ActiveRegister view page , 範本: empty
	- 實作登入/登出功能
		- 只有帳密正確且 IsConfirmed = true ,IsDeleted = false , IsBlackList = false 的會員才被允許登入，請事先準備已/未開通的會員各一，方便測式
	    - 修改 web.config，加入表單認證(cookie) => 
			 ``` 
			<authentication mode="Forms">
			<forms name="mySite" loginUrl="~/Members/Login" defaultUrl="~/Members/Index/" />
			</authentication>
			 ```
		- 加入 /Models/ViewModels/LoginVm class
		- 修改 MembersController , 加入 Login action
		- 加入 Login view page, 範本: create
		- 暫時做一個簡單的會員中心頁 /Members/Index
		- 修改 MemberControler, 加入 Logout action
		- 修改 _Layout.csthml, add登入/登出的連結(沒登入與已登入，要顯示不同連結)
	- 實作修改個資
		- 在 /Members/Index(會員中心) 加入修改個資的連結
		- 加入 /Models/ViewModels/ProfileVm class
		- 加入 /Members/Profile action
		- 加入 Profile view page, 範本: Edit 		
	- 實作變更密碼
		- 在/Members/Index(partial view)加入變更密碼的連結
		- 加入 /Models/ViewModels/ChangePasswordVm
		- 加入 /Members/ChangePassword action , 加入[Authorize]
		- 加入 ChangePassword view page , 範本: Create(不使用Edit，不需要事先load資料進去)
	- 實作忘記密碼/重設密碼
		- 加入 /Models/ViewModels/ForgotPasswordVm class
		- 加入 /Members/ForgotPassword action
		- 加入 ForgotPassword view page，範本: Create
		- 比對account , email ，若正確就 update confirmCode = guid ，並寄送Email (未做)
		- postback 成功後，return View("ConfirmForgotPassword")
		- 加入 /Models/ViewModels/ResetPasswordVm class,包括 update , confirmPassword
		- 加入 /Models/ResetPassword action ,  url = /Members/ResetPassword?memberId=99&confirmCode=XXXXXXXXXXX
		- 加入 ResetPassword view page , 範本: Create
		- 判斷memberId , confirmCode是否正確，若正確就update password , confirmCode = null
	- 實作取消會員功能
		- 在 ProfileVm加入 public bool IsDeleted { get; set; }
		- 加入的DeleteMember action 以及 修改 EditProfile view page，使得該頁 httppost 提交表單使用兩個不同的 action
		- 按下取消會員後導到 Logout action
	- 其他 
		- 使用sweetalert : 帳號開通頁， 編輯資料頁
- ## ServerSide
   ### 資料庫  
	- 加入 /Models/EFModels

   ### 後台模板
	- 修改 _Layout.cshtml, 加入後台模板的模板
	- 新增 模板到 wwwroot/css, wwwroot/js, /img
	- 新增 測試程式到 /Views/Movies/List.cshtml 當測試面板進行修改。
	- 修改 /Views/Home/Index.cshtml, 加入後台模板的模板
   ### 電影管理
	#### 1 . 電影清單系統
	- 加入 /Controllers/MoviesController，依賴介面。
		- 加入ConvertToDto, ConvertToVm方法。
	- 加入MovieVm跟MovieDto /Models/ViewModels/MovieVm , /Models/Dtos/MovieDto。
	- 建立Service,Dao介面並實作，並在Program.cs中註冊。
		- 加入GetGenresName , GetRatingsName方法，透過ViewBag傳遞Name到Index , Edit。
	- 實作CRUD Action，並建立對應View。
		- IndexPage可以點擊TableRow導向到EditPage。
	#### 2 . 電影種類系統
	- 加入 /Controllers/GenresController，依賴介面。
		- 實作IndexPage(範本List)、CreatePage(範本Create)、EditPage(範本Edit)。
	- 加入 GenreVm,GenreDto / IGenreService,IGenreDao並實作、註冊。
	- 實作CRUD Action，建立對應View。
	#### 3. 電影評論系統
	- 加入 /Controllers/ReviewsController，依賴介面。
		- 實作IndexPage(範本List)、EditPage(範本Edit)、CreatePage(範本Create)。
	- 加入 ReviewVm,ReviewDto / IReviewService,IReviewDao並實作、註冊。
	- 實作CRUD Action，建立對應View。
   ### 電影管理
	- 
	- 建立PriceVm、PriceDto、IPriceService、IPriceDao
   ### 會員系統
	- 加入 Models/Infra/HashUtility.cs (用來做密碼雜湊的公用函式)
	- 註冊hashutility的salt值，並在DI中註冊 => 
	 ```
      Configure App Configuration
            var configuration = builder.Configuration;
            HashUtility.SetConfiguration(configuration); 
	```
    - MemberService , MemberDao , MemberIndexVm , MemberCreateVm ,MemberDto 
	- 加入 MembersController Index action , /Views/Members/Index page , 未加入依賴介面
	- 加入 MembersController Edit action , view page
	- 加入 MembersController Create action, view page
	- 加入 sweetalert
   ### 員工系統
	-	 

   ### 票種系統
    - 加入 TicketsController ， 未加入依賴介面
	- 加入 TicketVm、TicketDto、TicketDao
	- 在 program.cs 中註冊
	- 在 TicketsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
	
   ### 票務座位系統
	- 加入 TicketSeatsController ， 未加入依賴介面
	- 加入 TicketSeatVm、TicketSeatDto、TicketSeatDao
	- 在 program.cs 中註冊
	- 在 TicketSeatsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯

	### 座位系統
	- 加入 SeatsController ， 未加入依賴介面
	- 加入 SeatVm、SeatDto、SeatDao
	- 在 program.cs 中註冊
	- 在 SeatsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯

	### 座位狀態系統
	- 加入 SeatStatussController ， 未加入依賴介面
	- 加入 SeatStatusVm、SeatStatusDto、SeatStatusDao
	- 在 program.cs 中註冊
	- 在 SeatStatussController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯

	### 優惠卷系統
	- 加入 CouponsController ， 未加入依賴介面
	- 加入 CouponVm、CouponDto、CouponDao
	- 在 program.cs 中註冊
	- 在 CouponsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
