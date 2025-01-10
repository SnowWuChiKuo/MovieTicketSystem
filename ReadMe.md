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
	- 實作會員確認功能  (已做發送email信開通帳號功能)
		- 加入 /Members/ActiveRegister, url是 /Members/ActiveRegister?memberId=&confirmCode=
		- 加入 ActiveRegister view page , 範本: empty
		- 加入 /Models/Infra/EmailService class
		- 在 Webconfig 中加入 smtp 設定 =>
		```
		<!-- smtp 設定-->
		<add key="SmtpServer" value="smtp.gmail.com" />
		<add key="SmtpPort" value="587" />
		<add key="SmtpUsername" value="mailsendersimulate1@gmail.com" />
		<!-- 替換成你的 Gmail 帳號 -->
		<add key="SmtpPassword" value="hjky lpxr qedj krjv" />
		<!-- 替換成你的 Gmail 應用程式密碼 -->
		<add key="EnableSsl" value="true" /> 
		```
		- 修改 MembersController/Register action ，加入寄送 Email 功能
		- 使用者點擊就會導到 ActiveRegister action，帳號成功開通
		
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
		- 修改 _Layout.csthml, add登入/登出的連結(沒登入與已登入，會顯示不同連結)
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
	- 實作忘記密碼/重設密碼 ( 已做寄送 Email 導回重設密碼頁 )
		- 加入 /Models/ViewModels/ForgotPasswordVm class
		- 加入 /Members/ForgotPassword action
		- 加入 ForgotPassword view page，範本: Create
		- 比對account , email ，若正確就 update confirmCode = guid ，並寄送Email
		- postback 成功後，return View("ConfirmForgotPassword")
		- 使用者點擊 Email 連結後導到 ResetPassword action 以重設密碼
		- 加入 /Models/ViewModels/ResetPasswordVm class,包括 update , confirmPassword
		- 加入 /Models/ResetPassword action ,  url = /Members/ResetPassword?memberId=99&confirmCode=XXXXXXXXXXX
		- 加入 ResetPassword view page , 範本: Create
		- 判斷memberId , confirmCode是否正確，若正確就update password , confirmCode = null
	- 實作取消會員功能
		- 在 ProfileVm加入 public bool IsDeleted { get; set; }
		- 加入的DeleteMember action 以及 修改 EditProfile view page，使得該頁 httppost 提交表單使用兩個不同的 action
		- 按下取消會員後導到 Logout action
	- 其他 
		- 使用sweetalert : 帳號開通頁， 編輯資料頁， 重設密碼頁

	### 訂單系統
    - 實作訂單系統功能:
		- 加入 CartController ，確認訂單 Checkout() Get Post 、得到購物車資料 GetCartInfo()。
		- 加入 CartVm、CheckoutVm， CartVm 裡面包含 CartItem 類似，CheckoutVm 裡面是 Order 類似。
		- 加入 CartService，加入確認購物車資料、創建訂單以及刪除購物車。
		- 加入 CartEFDao、OrderEFRepository，將訂單建立以及刪除購物車。
		- 加入 Cart 的 Index View 頁，選擇影廳、場次時間、票種、價格。


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
	- 加入 ```/Controllers/MoviesController```，依賴介面。
		- 加入ConvertToDto, ConvertToVm方法。
	- 加入MovieVm跟MovieDto ```/Models/ViewModels/MovieVm``` + ```/Models/Dtos/MovieDto```。
	- 建立Service,Dao介面並實作，並在Program.cs中註冊。
		- 加入GetGenresName , GetRatingsName方法，透過ViewBag傳遞Name到Index , Edit。
	- 實作CRUD Action，並建立對應View。
		- IndexPage可以點擊TableRow導向到EditPage。
	#### 2 . 電影種類系統
	- 加入 ```/Controllers/GenresController```，依賴介面。
		- 實作IndexPage(範本List)、CreatePage(範本Create)、EditPage(範本Edit)。
	- 加入 GenreVm,GenreDto / IGenreService,IGenreDao並實作、註冊。
	- 實作CRUD Action，建立對應View。
	#### 3. 電影評論系統
	- 加入 ```/Controllers/ReviewsController```，依賴介面。
		- 實作IndexPage(範本List)、EditPage(範本Edit)、CreatePage(範本Create)。
	- 加入 ReviewVm,ReviewDto / IReviewService,IReviewDao並實作、註冊。
	- 實作CRUD Action，建立對應View。
   ### 電影票券管理
	- 加入 ```/Controllers/PricesController```，依賴介面。
		- 實作IndexPage(範本List)、DetailsPage(範本List)、EditPage(範本Edit)、CreatePage(範本Create)。
	- 建立PriceVm、PriceDto、IPriceService、IPriceDao
	- 實作CRUD Action，建立對應View。
   ### 影廳管理系統
	- 加入 ```/Controllers/TheatersController```，依賴介面。
	- 加入 TheaterVm,TheaterDto / ITheaterService,ITheaterDao並實作、註冊。
	- 實作Search、Edit Action，建立對應View。
   ### 場次系統
	- 加入 ```/Controller/ScreeningController```，依賴介面。
	- 加入 ScreeningVm,ScreeningDto / IScreeningService,IScreeningDao並實作、註冊。
	- 實作CRUD Action，建立對應View。
	---
   ## 增加自訂驗證
	- 建立 Attributes 資料夾
	```/Models/Attributes/ValidScreeningDateAttribute```
	- ViewModel加入自訂驗證
		```
		public class ScreeningEditVm
		{
			[ValidScreeningDate]  // 套用自定義驗證
			public DateTime ScreeningDate { get; set; }
		} 
		```
	- 確定前端ViewPage引用驗證
		```
		@section Scripts {
			@{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
		}
		```
	-**目的**
	```
	- 用於驗證場次日期是否符合電影上映日期規則
	- 確保場次日期不會早於電影的上映日期
	```
	-**實作**
	```
	- 繼承 ValidationAttribute
	- Override 改寫IsValid 方法
	- 注入服務（IScreeningService）
	```
	---
   ### 會員系統
	- 加入 Models/Infra/HashUtility.cs (用來做密碼雜湊的公用函式)
	- 在 appsetting.json 註冊 hashutility 的 salt 值 =>
	  ```
      "AppSettings": {
      "salt": "s-kf!d1(#!$#elwpq2"
      }
	  ```
	- 在DI中註冊 => 
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

	### 購物車系統
	- Cart:
	- 加入 CartsController ， 未加入依賴介面
	- 加入 CartVm、CartDto、CartDao
	- 在 program.cs 中註冊
	- 在 CartsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
	
	- CartItem:
	- 加入 CartItemsController， 未加入依賴介面
	- 加入 CartItemVm、CartItemDto、CartItemDao、CartItemService
	- 加入Index view page 
	- 加入 Create view page
	- 加入 Edit view page (目前有無法判斷整個購物車項目總數不能超過6的bug)
	

	### 訂票系統
	- 加入 OrdersController ， 未加入依賴介面
	- 加入 OrderVm、OrderDto、OrderDao
	- 在 program.cs 中註冊
	- 在 OrdersController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
	- `優惠卷折扣後金額未製作`

	### 訂票細項系統
	- 加入 OrderItemsController ， 未加入依賴介面
	- 加入 OrderItemVm、OrderItemDto、OrderItemDao
	- 在 program.cs 中註冊
	- 在 OrderItemsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
	- `限制購買數量為六張`

	### 優惠卷系統
	- 加入 CouponsController ， 未加入依賴介面
	- 加入 CouponVm、CouponDto、CouponDao
	- 在 program.cs 中註冊
	- 在 CouponsController 寫入 CRUD ，並顯示其 View
	- View 的 Index頁(範本List)、Create頁(範本Create)、Edit頁(範本Edit)
	- 刪除部分在 Edit 頁， List 頁僅可編輯
	
