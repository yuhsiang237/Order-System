# order-system
商品出退貨訂單系統
### Setup
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
