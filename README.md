#使用群組性或單一電話發送GSM#

# 介紹 #

### GSM是甚麼? ###
全球行動通訊系統（Global System for Mobile Communications)

### GSM優勢 ###
**GSM**優勢在於提供更高的數位語音品質和替代呼叫的低成本的新選擇（比如簡訊）。<br>從網路業者角度看來，其優勢是能夠部署來自不同廠商的裝置，因為GSM作為開放標準提供了更容易的互操作性。

### GSM又稱身分識別模組 ###
身分識別模組(SIM)是一個儲存使用者資料和電話本的可拆卸智慧卡IC。<br>使用者可更換手機後還能儲存自己的資訊。換句話說使用者也可以使用現在的手機而使用不同業者的SIM卡。

### GSM安全機制 ###
GSM被設計具有中等安全水平。系統設計使用共享金鑰使用者認證。使用者與基站之間的通訊可以被加密。演進的UMTS引入可選的USIM－使用更長鑑別金鑰保證更好的安全以及網路和使用者的雙向驗證。

# 硬體設備 #

- 3G Modem x 1
- Sim Card x 1

# Server #

執行時,系統會一直等待**Client**給予**Server**訊號,此訊號傳入Server時會分類為三類

1. IP:<br>
Server接收到Client訊號時，如IP非Access中名單就無法進入**群組**或**電話**類別。

1. 群組:<br>
檢測Client傳入的群組是否與Access內部群組相同，<br>
如為真將會依照順序發送名單，如為否系統會自動刪除此訊息。

1. 電話:<br>
檢測Client傳入的電話號碼是否與Access內部電話相同，<br>
如為真將立即發送此號碼，如為否系統會自動刪除此訊息。


## Server執行步驟

### 圖片: ###

第一步:

	
第二步:

### Access修改: ###

第一步(修改內部AccessPwd):

![AccessPwd](https://github.com/nexstar/GSM/raw/master/Picture/ServerCode1.PNG)

第二步(請先輸入預設Pwd:@1436)，未來要更換密碼請務必記得內部需更換:

![AccessPwd](https://github.com/nexstar/GSM/raw/master/Picture/AccessLockcode.PNG)

第三步(設定群組與電話名單):

![AccessPwd](https://github.com/nexstar/GSM/raw/master/Picture/AddressBook.PNG)
		
第四步(設定受限IP，請務必記得此IP屬於可通過者):

![AccessPwd](https://github.com/nexstar/GSM/raw/master/Picture/IP.PNG)


# Client #

執行方式為使用命令提示(CMD) 做為傳送並且帶參數，參數種類為以下四選一

1. Phone_"Text Massage"

2. Phone,Group_"Text Massage"

3. Group,Group_"Text Massage"

4. Group_"Text Massage"

註解:底線為空格

除以上也可依照您的方式搭配參數,以上四種為基本參數，請務必記得如參數為一個以上請在後方加入逗點。

## Client執行步驟

第一步(修改內部):	 
![修改內部](https://github.com/nexstar/GSM/raw/master/Picture/ClientCode.PNG)

第二步(執行時輸入參數示意圖): 
![執行時輸入參數示意圖](https://github.com/nexstar/GSM/raw/master/Picture/Client1.PNG)

第三步(結果示意圖):
![示意圖](https://github.com/nexstar/GSM/raw/master/Picture/Client.PNG)

# Registry #

本專案使用Registry原因為利用此方式當Server重開時會自動抓取Comport。

# 資料參考 #
[https://zh.wikipedia.org/wiki/GSM](https://zh.wikipedia.org/wiki/GSM "GSM( WIKI )")

# 製作者 #

NianBaoZou	( nexstar@gmail.com )<br>
3/17/2016 3:29:41 PM 