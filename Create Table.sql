﻿CREATE DATABASE LibraryCms
GO
CREATE LOGIN admin WITH PASSWORD = 'admin',DEFAULT_DATABASE = LibraryCms
GO
USE LibraryCms
GO
CREATE USER admin FOR LOGIN admin
EXEC sp_addrolemember 'db_owner', 'admin'
GO
CREATE TABLE tb_Role(
	[RoleID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[RoleName] VARCHAR(50) NOT NULL UNIQUE,
	[DepartmentType] CHAR NULL,
	[DepartmentID] INT NULL,
	[Rights] CHAR(6) NOT NULL
)
CREATE TABLE tb_User(
	[UserID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Number] VARCHAR(8) NOT NULL UNIQUE,
	[Password] VARCHAR(32) NOT NULL,
	[RoleID] INT NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Mail] VARCHAR(50) NULL,
	[Phone] CHAR(11) NULL,
	[QQ] VARCHAR(20) NULL,
	[Sex] CHAR(1) NULL,
	FOREIGN KEY([RoleID]) REFERENCES tb_Role([RoleID])
)
CREATE TABLE tb_UserSettings(
	[UserID] INT NOT NULL PRIMARY KEY,
	[FontFamily] VARCHAR(50) NOT NULL,
	[FontSize] INT NOT NULL,
	[FontWeight] VARCHAR(10) NOT NULL,
	[FontColor] VARCHAR(7) NOT NULL,
	[BackgroundColor] VARCHAR(7) NOT NULL,
	FOREIGN KEY([UserID]) REFERENCES tb_User([UserID])
)
CREATE TABLE tb_Department_X(
	[DepartmentID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DepartmentName] VARCHAR(50) NOT NULL UNIQUE
)
CREATE TABLE tb_Department_B(
	[DepartmentID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DepartmentName] VARCHAR(50) NOT NULL UNIQUE
)
CREATE TABLE tb_Department_A(
	[DepartmentID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DepartmentName] VARCHAR(50) NOT NULL UNIQUE
)
CREATE TABLE tb_Type(
	[TypeID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ParentID] INT NULL,
	[TypeName] VARCHAR(50) NOT NULL UNIQUE,
	[TypePath] VARCHAR(100) NOT NULL,
	FOREIGN KEY([ParentID]) REFERENCES tb_Type([TypeID]) 
)
CREATE TABLE tb_Book(
	[BookID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[BookName] VARCHAR(100) NOT NULL,
	[Author] VARCHAR(50) NOT NULL,
	[Publisher] VARCHAR(50) NULL,
	[Pages] INT NOT NULL,
	[PublicTime] VARCHAR(50) NOT NULL,
	[Formart] VARCHAR(20) NOT NULL,
	[BookPath] VARCHAR(100) NOT NULL,
	[DownloadNumber] INT NOT NULL DEFAULT(0),
	[Point] FLOAT NOT NULL DEFAULT(0),
	[DepartmentID] INT NOT NULL,
	FOREIGN KEY([DepartmentID]) REFERENCES tb_Department_B([DepartmentID]) 
)
CREATE TABLE tb_BookType(
	[BookID] INT NOT NULL,
	[TypeID] INT NOT NULL,
	PRIMARY KEY([BookID],[TypeID]),
	FOREIGN KEY([TypeID]) REFERENCES tb_Type([TypeID]),
	FOREIGN KEY([BookID]) REFERENCES tb_Book([BookID])
)
CREATE TABLE tb_Message_Admin(
	[MessageID] INT IDENTITY(1,1),
	[From] INT NOT NULL,
	[Time] DATETIME NOT NULL,
	[Subject] TEXT NOT NULL,
	[Content] TEXT NULL,
	[Status] INT NOT NULL,
	[HandleAdminID] INT NOT NULL
)
CREATE TABLE tb_Message_1(
	[MessageID] INT IDENTITY(1,1)NOT NULL PRIMARY KEY,
	[From] INT NOT NULL,
	[Time] VARCHAR(20) NOT NULL,
	[Subject] TEXT NOT NULL,
	[Content] TEXT NULL,
	[Status] INT NOT NULL,
	FOREIGN KEY([From]) REFERENCES tb_User([UserID]) 
)
CREATE TABLE tb_Message_2(
	[MessageID] INT IDENTITY(1,1)NOT NULL PRIMARY KEY,
	[From] INT NOT NULL,
	[Time] VARCHAR(20) NOT NULL,
	[Subject] TEXT NOT NULL,
	[Content] TEXT NULL,
	[Status] INT NOT NULL,
	FOREIGN KEY([From]) REFERENCES tb_User([UserID]) 
)
CREATE TABLE tb_Message_3(
	[MessageID] INT IDENTITY(1,1)NOT NULL PRIMARY KEY,
	[From] INT NOT NULL,
	[Time] VARCHAR(20) NOT NULL,
	[Subject] TEXT NOT NULL,
	[Content] TEXT NULL,
	[Status] INT NOT NULL,
	FOREIGN KEY([From]) REFERENCES tb_User([UserID]) 
)
CREATE TABLE tb_Question_1(
	[QuestionID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Content] VARCHAR(50) NOT NULL,
	[answerA] VARCHAR(50) NOT NULL,
	[answerB] VARCHAR(50) NULL,
	[answerC] VARCHAR(50) NULL,
	[answerD] VARCHAR(50) NULL,
	[answerE] VARCHAR(50) NULL,
	[Correct] CHAR(5) NOT NULL,
	[Type] INT NOT NULL
)
CREATE TABLE tb_Question_2(
	[QuestionID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Content] VARCHAR(50) NOT NULL,
	[answerA] VARCHAR(50) NOT NULL,
	[answerB] VARCHAR(50) NULL,
	[answerC] VARCHAR(50) NULL,
	[answerD] VARCHAR(50) NULL,
	[answerE] VARCHAR(50) NULL,
	[Correct] CHAR(5) NOT NULL,
	[Type] INT NOT NULL
)
GO
CREATE FUNCTION MD5(@src varchar(255) )
RETURNS varchar(255)
AS
BEGIN
    DECLARE @md5 varchar(34)
    SET @md5 = sys.fn_VarBinToHexStr(hashbytes('MD5', @src));
    --RETURN SUBSTRING(@md5,11,16)   --16位
    RETURN SUBSTRING(@md5,3,32)    --32位
END
GO
USE LibraryCms
INSERT INTO tb_Department_A VALUES('超级管理员')
INSERT INTO tb_Department_B VALUES('数学部')
INSERT INTO tb_Department_X VALUES('计算机系')
INSERT INTO tb_Role VAlUES('超级管理员组','A',1,'111111')
INSERT INTO tb_Book VALUES('Effective C++ 第三版（英文）','Scott Meyers',NULL,297,'2006-07','pdf','A94069B3A7259496EAAE59EC35878B2A',0,0,1)
INSERT INTO tb_Book VALUES('布拉格红人馆','桃子夏',NULL,83,'2008-03','txt','9B1F593FE60E5079DC24845A8BBB2D3A',0,0,1)
INSERT INTO tb_User VALUES('00000000',dbo.MD5('admin'),1,'超级管理员1',NULL,NULL,'769655297',NULL)
INSERT INTO tb_User VALUES('00000001',dbo.MD5('admin'),1,'超级管理员2','test@test.com',NULL,NULL,NULL)
INSERT INTO tb_User VALUES('00000002',dbo.MD5('admin'),1,'超级管理员3',NULL,'15100008937',NULL,NULL)

