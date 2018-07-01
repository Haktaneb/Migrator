CREATE TABLE [User]  

   (Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
    FullName nvarchar(25) NOT NULL,  
    Email nvarchar(30));

GO

CREATE Table [House]
(
  
  
			Id int IDENTITY(1,1) PRIMARY KEY (Id) NOT NULL,
            Address nvarchar(25) NOT NULL,
			UserId int ,
			CONSTRAINT FK_UserHouse FOREIGN KEY (UserId)
			REFERENCES [User](Id)
   );
GO