"# InternalProject" 

##Description

Binary base is a form of electronic commerce that allows consumers to directly buy goods from the seller over the internet. What separates our website to other ecommerce website? We are local and customers can make decision to buy the product online or visit a local market where the product is being sold. This project will help to those people who likes shopping but wants doesn't want to go to the mall or store. This will save some time and comfortable for them to do their shopping time. It's also provides good deals of the week on our featured products.

##Functionalities

##Essential

Admin can utilized the project, and can manage accounts and products
Sellers can login and manage their listings
Sellers can create/read/update/delete products
Users can search products (by category, search bar, featured list, store, location)
Users can do their shopping by choosing the products that they want and add into their cart.
Only users can do finish their online shopping by signing in and proceed to checkout.
Guests user has a privillege to check awesome products and good deals in this project and requiring them to create an account or sign up for free.

Important

Product description, specification, etc.
Users information
Sellers can choose to input url or add images (to database).

##Diagrams

 
##Web API (For Devs)

We are developing a web API to our website to enable other developers to use them to enahnce their experience using our website. To learn more about using our web API, click here.

##Current API:

Get products from specific vendor.
SQL

 IF OBJECT_ID('ProductOrder', 'U')    
    IS NOT NULL DROP TABLE ProductOrder ;
	
IF OBJECT_ID('Cart', 'U')
	IS NOT NULL DROP TABLE Cart ;

IF OBJECT_ID('OrderTable', 'U')    
    IS NOT NULL DROP TABLE OrderTable ;

IF OBJECT_ID('Product', 'U')    
    IS NOT NULL DROP TABLE Product;

IF OBJECT_ID('Address', 'U')    
    IS NOT NULL DROP TABLE Address ;

IF OBJECT_ID('UserTable', 'U')    
    IS NOT NULL DROP TABLE UserTable;

IF OBJECT_ID('Vendor', 'U')    
    IS NOT NULL DROP TABLE Vendor ;

IF OBJECT_ID('Account ', 'U')    
    IS NOT NULL DROP TABLE Account ;

CREATE TABLE     Account ( 
    userName      varchar(50) PRIMARY KEY,
    accountType    INT );

CREATE TABLE     Vendor ( 
    userName      varchar(50) FOREIGN KEY REFERENCES Account(userName ),
    vendorName    varchar(100),  
    firstName        varchar(50),  
    lastName        varchar(50),  
    phone        varchar(20),  
    PRIMARY KEY (userName));

CREATE TABLE     UserTable( 
    userName      varchar(50 ) FOREIGN KEY REFERENCES Account (userName ),
    firstName        varchar(50),  
    lastName        varchar(50),  
    phone        varchar(20),  
    PRIMARY KEY (userName));

CREATE TABLE     Address( 
    addressID    INT IDENTITY PRIMARY KEY,  
    houseNumber    varchar(10),  
    street        varchar(50),  
    city        varchar(50),  
    province        varchar(50),  
    userName      varchar(50) FOREIGN KEY REFERENCES Account (userName ),
) ;

CREATE TABLE     Product( 
    productID    INT IDENTITY PRIMARY KEY,  
    name		 varchar(100),  
    price        decimal(9,2),
    imgUrl       varchar(200),  
    discountPrice    decimal(9,2),
	summary		varchar(2000),
	manufacturer varchar(50),
    userName    varchar(50) FOREIGN KEY REFERENCES Vendor(userName )
) ;

CREATE  TABLE     OrderTable( 
    orderID        INT IDENTITY PRIMARY KEY,
    timeStamp	 DATETIME,  
    userName      varchar(50) FOREIGN KEY REFERENCES UserTable(userName ),
    addressID       INT   FOREIGN KEY REFERENCES Address(addressID )
    );


CREATE  TABLE     ProductOrder( 
    orderID        INT FOREIGN KEY REFERENCES OrderTable(orderID ),
    productID       INT FOREIGN KEY REFERENCES Product(productID ),
    quantity        INT,
    PRIMARY KEY (orderID, productID)
    );

CREATE TABLE Cart(
	productID	INT FOREIGN KEY REFERENCES Product(productID),
	userName	varchar(50) FOREIGN KEY REFERENCES UserTable(userName),
	quantity	INT,
	PRIMARY KEY (productID,userName)
);
	
INSERT INTO Account VALUES ('admin',0);
INSERT INTO Account VALUES ('seller',1);
INSERT INTO Account VALUES ('seller0',1);
INSERT INTO Account VALUES ('user',2);
INSERT INTO Account VALUES ('user0',2);

insert into UserTable (userName,firstName,lastName,phone) Values ('user','Casey','Miller','604 123-1234')
insert into UserTable (userName,firstName,lastName,phone) Values ('user0','Julius','Greek','604 325-5555')

insert into Vendor (userName,vendorName,firstName,lastName,phone) Values ('seller','FastBuy','Shane','Kim','778 839-8673')
insert into Vendor (userName,vendorName,firstName,lastName,phone) Values ('seller0','BestPurchase','Kane','Shane','778 839-4256')

insert into Address (houseNumber,street,city,province,userName) Values ('123','Main Street','Vancouver','BC','user')
insert into Address (houseNumber,street,city,province,userName) Values ('365','Main Street','Vancouver','BC','user0')
insert into Address (houseNumber,street,city,province,userName) Values ('160','SW Marine Dr','Vancouver','BC','seller')
insert into Address (houseNumber,street,city,province,userName) Values ('160','SW Marine Dr','Vancouver','BC','seller0')

insert into Product (name,price,imgUrl,discountPrice,summary,manufacturer,userName) Values ('Mac mini',599.99,'seller/mac_mini.jpg',499.99,'Mac mini is an affordable powerhouse that packs the entire Mac experience into a 19.7-cm-square frame. Just connect your own display, keyboard and mouse, and you’re ready to make big things happen.','Apple','seller')
insert into Product (name,price,imgUrl,discountPrice,summary,manufacturer,userName) Values ('Macbook air',1199.99,'seller/macbook_air.jpg',1099.99,'MacBook Air lasts up to an incredible 12 hours between charges. So from your morning coffee till your evening commute, you can work unplugged. When it’s time to kick back and relax, you can get up to 12 hours of iTunes movie playback. And with up to 30 days of standby time, you can go away for weeks and pick up right where you left off.','Apple','seller')
insert into Product (name,price,imgUrl,discountPrice,summary,manufacturer,userName) Values ('Macbook pro',1549.99,'seller/macbook_pro.jpg',1349.99,'The new MacBook Pro is faster and more powerful than before, yet remarkably thinner and lighter. It has the brightest, most colourful display ever on a Mac notebook. And it features up to 10 hours of battery life. It''s a notebook built for the work you do every day. Ready to go anywhere a great idea takes you.','Apple','seller0')

insert into Cart values(1,'user',2);
insert into Cart values(2,'user',1);

insert into OrderTable(username,timeStamp) values('user','2016-09-12 09:00:00');
insert into OrderTable(username,timeStamp) values('user0','2017-03-12 19:00:00');
insert into ProductOrder values(1,1,3);
insert into ProductOrder values(2,2,4);

delete from AspNetUsers
delete from AspNetRoles
delete from AspNetUserRoles

insert into AspNetUsers values('8113f10f-1d5e-4a04-9c57-ff4a650c6db9','admin@gmail.com',1,'AJrolT735SeILuadEnctwBEYyfaESIMHLyyT8RXDaC9dT7kw/MmU2D6uO3uRGD0FBQ==','df4b3332-cba5-4157-9e12-025698683a61',NULL,0,0,NULL,1,0,'admin');
insert into AspNetUsers values('34661167-b39c-40a0-962e-ffac618d6f7c','seller@gmail.com',1,'AKuNznvrpeGPGQUacHnI+P1nAgY1o4j5fadN6tZFRjplONGF6itjidPrqLe1L6Kiqw==','60f0b2e0-836e-4005-a7d1-9893ecac18bc',NULL,0,0,NULL,1,0,'seller');
insert into AspNetUsers values('d66ca066-7d93-4601-9eb4-30a774d26d40','user@gmail.com',1,'ANOnYpFeOL75UAnyzghZzdyWj2GQCtyPt2vbYXzQBwvUthTsXKBYUVjD7IQ6tDnNnw==','35662f2f-0553-47a5-b2a1-d392f282aef8',NULL,0,0,NULL,1,0,'user');
insert into AspNetUsers values('7d9404da-6216-4a6f-948d-ed3d0a335691','user0@gmail.com',1,'AGZCvRIch3a97L3T7N7Hbf9TCaotQ86Ih6K6QQzylb/u7SZ+iDFnXZM8CE5heVWMww==','81f7eb92-ee67-4994-a1bf-dd27d63a2dcd',NULL,0,0,NULL,1,0,'user0');

insert into AspNetRoles values(0,'Admin');
insert into AspNetRoles values(1,'Seller');
insert into AspNetRoles values(2,'User');

insert into AspNetUserRoles select Id,0 from AspNetUsers where(UserName = 'admin');
insert into AspNetUserRoles select Id,1 from AspNetUsers where(UserName = 'seller');
insert into AspNetUserRoles select Id,1 from AspNetUsers where(UserName = 'seller0');
insert into AspNetUserRoles select Id,2 from AspNetUsers where(UserName = 'user');
insert into AspNetUserRoles select Id,2 from AspNetUsers where(UserName = 'user0');
