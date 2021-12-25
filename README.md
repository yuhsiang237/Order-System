# 商品出退貨訂單系統 (OrderSystem)
<img style="width:250px" src="https://github.com/yuhsiang237/Order-System/blob/master/doc/8.美術設計稿/demofile/6.png" />

### 說明(Intro)
主要用來出退貨商品訂單，能夠建立出貨、退貨訂單，並具備商品庫存機制、多角色權限、權限限制(新、刪、修、查)等頁面的細微權限。  
並且使用到ASP.NET Core MVC、Entity Framework、Vue.js、Jquery、Bootstrap、SQL Server進行開發。  
在資料庫規劃上也考量到現實情況，亦有多對多關聯的設計。  

### 系統安裝與啟用(Setup)
可以直接參考此 [完整安裝影片](https://github.com/yuhsiang237/Order-System/blob/master/doc/9.%E7%B3%BB%E7%B5%B1%E8%B3%87%E6%BA%90%E6%AA%94%E6%A1%88/%E7%B3%BB%E7%B5%B1%E5%BB%BA%E7%BD%AE%E6%95%99%E5%AD%B8%E5%BD%B1%E7%89%87.mp4)，協助搭建，約5分鐘。
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
