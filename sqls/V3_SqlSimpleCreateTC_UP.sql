ALTER TABLE	[User]	

	Add  gender nvarchar(5);
    
GO

CREATE Table [Room]
(
  
  
			Id int IDENTITY(1,1) PRIMARY KEY (Id) NOT NULL,
            WindowNumber int,
			Size nvarchar(30),
			HouseId int ,
			CONSTRAINT FK_RoomOfTheHouse FOREIGN KEY (HouseId)
			REFERENCES [House](Id)
   );
GO