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
	- 加入 ReviewVm,ReviewDto / IReviewService,IReviewDao並實作、註冊。
   ### 會員系統
	- 加入 MembersController 和 /Views/Members
	- 加入 Models/Infra/HashUtility.cs (用來做密碼雜湊的公用函式)

   ### 票種系統
    - 加入 TicketsController ， 未加入依賴介面
	- 加入 TicketVm、TicketDto、TicketDao
	- 在 program.cs 中註冊
	- 在 TicketsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
	
   ### 票務座位系統
	- 加入 TicketSeatsController ， 未加入依賴介面
	- 加入 TicketVm、TicketDto、TicketDao
	- 在 program.cs 中註冊
	- 在 TicketsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
