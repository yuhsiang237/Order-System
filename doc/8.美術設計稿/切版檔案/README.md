# 說明

{{{{[專案安裝有對有錯，使用前請先詳閱公開說明書。}}}}}


##檔案裡的package-lock.json參考用，初始安裝前還是先刪除
##以下處理完後請再重新npm install呦
##pug沒刪看哪天有人想用，用不到的可以事先刪除


#gulp-imagemin
在新一版裡寫法有變更：jpegtran =====> mozjpeg (對只有這個差別)

#node-sass
如果node-sass版本太舊還是啟動時有檔案找不到請重建 =====> npm rebuild node-sass

#core-js
現在版本是@2，如果core-js哪天說了要完全升級那就 =====>  npm install --save core-js@^3

#autoprefixer (Postcss)
塞回去了，現在應該有了...