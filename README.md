# 商品出退貨訂單系統 (OrderSystem)
<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/6.png" />

### 說明(Intro)
主要用來出退貨商品訂單，能夠建立出貨、退貨訂單，並具備商品庫存機制、多角色權限、權限限制(新、刪、修、查)等頁面的細微權限。  
並且使用到ASP.NET Core MVC、Entity Framework、Vue.js、Jquery、Bootstrap、SQL Server進行開發。  
在資料庫規劃上也考量到現實情況，亦有多對多關聯的設計。  
為保護帳號安全，密碼採用HASH與加鹽方式儲存。  
資料驗證欄位採用Fluent Validation及部分Model Validation。

完整操作影片請看:[系統操作展示影片](https://www.youtube.com/watch?v=nSk6OZjhDMY) 

### 系統安裝與啟用(Setup)
可以直接參考此 [完整安裝影片](https://www.youtube.com/watch?v=-MZ5YEJWP2o)，協助搭建，約5分鐘。  
以及 [系統建置教學文件](doc/9.系統資源檔案/系統建置教學.txt)
```
1.從Github將專案Order-System下載下來。

2.建置資料庫，請注意有順序性，先匯入結構再來才是資料
  
  a.先匯入資料庫的結構 
  /doc/9.系統資源檔案/OrderSystemSchemaScript.sql
  b.再匯入基礎資料(角色權限、基礎帳號資料)
  /doc/9.系統資源檔案/OrderSystemDefaultDataScript.sql

3.開啟專案OrderSystem.sln

4.專案設定
將appsettings.example.json複製一份
更名為appsettings.json
並修改資料庫連線資串
"DBConnectionString": "Server=.\\SQLExpress;Database=OrderSystem;Trusted_Connection=True;ConnectRetryCount=0",

5.再來可用預設帳密做登入:
帳號:demouser
密碼:demopwd

6.完成
```

### 系統開發環境要求
- ASP.NET Core 3.1
- SQL Server 2019
- Visual Studio 2019

### 基礎內頁展示
完整操作影片請看:[系統操作展示影片](https://www.youtube.com/watch?v=nSk6OZjhDMY) 

<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/1.png" />
<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/2.png" />
<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/3.png" />
<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/4.png" />
<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/5.png" />
