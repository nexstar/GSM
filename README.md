#使用群組性或電話號碼條件發送GSM#

# 介紹 #
此專案為GSM(發送簡訊)，

專案中包含 **伺服器** 與 **客戶端** 兩種程式碼<br>
專案中利用兩大方式 **群組性**或**電話號碼**來解析客戶端傳送的資料是否存在於Access<br>
如資料確認為真系統會自動發出客戶端指定給予特殊對象的訊息,如資料確認為否此訊息將會自動刪除。

# 硬體設備 #

- 3G Modem x 1
- Sim Card x 1

# Server #

Server執行時,系統會一直等待**Client**給予**Server**訊號,此訊號傳入Server時會分類為三類

1. 群組:<br>
Server會利用此方式,檢測Client傳入的群組是否與Access內部資料為真，<br>
如為真Server會依照順序發送名單，如為否系統會自動刪除此訊息。

2. 電話:<br>
Server會利用此方式,檢測Client傳入的電話號碼是否與Access內部資料為真，<br>
如為真Server會發送名單，如為否系統會自動刪除此訊息。

3. IP:<br>
Server接收到Client訊號時，如IP非Access中就無法進入**群組**或**電話**類別。

## 執行步驟



# Client #

# Registry #

# 製作者 #

NianBaoZou	( nexstar@gmail.com )<br>
3/15/2016 1:00:18 AM 