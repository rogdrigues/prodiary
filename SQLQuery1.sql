USE [master];
GO

IF DB_ID('DiaryNote') IS NOT NULL 
BEGIN
	ALTER DATABASE [DiaryNoteTest] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [DiaryNoteTest];
END
CREATE DATABASE [DiaryNoteTest];
GO
USE [DiaryNoteTest];
GO



CREATE TABLE Account (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    FullName NVARCHAR(100),
    Avatar VARBINARY(MAX),
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Memo (
    MemoID INT IDENTITY(1,1) PRIMARY KEY,
    MemoTitle NVARCHAR(255) NOT NULL,
    MemoContent NVARCHAR(MAX) NOT NULL,
    MemoAttachments VARBINARY(MAX),
    MemoCreated DATETIME DEFAULT GETDATE(),
    MemoUpdated DATETIME DEFAULT GETDATE(),
    MemoAuthor NVARCHAR(255) NOT NULL
);

CREATE TABLE MemoAddition(
    MemoAdditionID INT IDENTITY(1,1) PRIMARY KEY,
    MemoID INT FOREIGN KEY REFERENCES Memo(MemoID),
    MemoAttachments VARBINARY(MAX),
    MemoContentAddition NVARCHAR(MAX)
);

CREATE TABLE Task (
    TaskID INT IDENTITY(1,1) PRIMARY KEY,
    TaskTitle NVARCHAR(255) NOT NULL,
    TaskContent NVARCHAR(MAX) NOT NULL,
    TaskStatus NVARCHAR(255),
	TaskDate DATETIME ,
    TaskBegin DATETIME ,
    TaskEnd DATETIME ,
    ID INT NOT NULL,
	FOREIGN KEY([ID])
	REFERENCES ACCOUNT([ID])
);

CREATE TABLE Tag (
    TagID INT IDENTITY(1,1) PRIMARY KEY,
    TagName NVARCHAR(255) NOT NULL
);

CREATE TABLE VerificationCode(
	VerifyID INT IDENTITY(1,1) PRIMARY KEY,
	Email NVARCHAR(100) NOT NULL,
	Code VARCHAR(6) NOT NULL,
	Created DATETIME DEFAULT GETDATE(),
)

CREATE TABLE MemoTag (
    MemoID INT,
    TagID INT,
    FOREIGN KEY (MemoID) REFERENCES Memo(MemoID),
    FOREIGN KEY (TagID) REFERENCES Tag(TagID)
);
CREATE TABLE [dbo].[PlayList](
	[ID] [int] NOT NULL,
	[PlayListName] [nvarchar](150) NULL,
	[Owner] [int] NULL
) ON [PRIMARY]
CREATE TABLE [dbo].[PlayListSong](
	[ID] [int] NOT NULL,
	[SongID] [int] NOT NULL,
	[PlayListID] [int] NOT NULL,
	[Status] [int] NULL
) ON [PRIMARY]
CREATE TABLE [dbo].[Singer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SingerName] [nvarchar](150) NULL,
 CONSTRAINT [PK_Singer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
CREATE TABLE [dbo].[Song](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LinkToFile] [nvarchar](max) NULL,
	[LinkToWeb] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Owner] [int] NULL,
	[Title] [nvarchar](200) NULL,
	[Category] [int] NULL,
	[Author] [int] NULL,
	[ImageURL] [nvarchar](500) NULL,
	[Time] [nvarchar](50) NULL,
 CONSTRAINT [PK_Song] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Song]  WITH CHECK ADD  CONSTRAINT [FK_Song_Account] FOREIGN KEY([Owner])
REFERENCES [dbo].[Account] ([ID])

ALTER TABLE [dbo].[Song] CHECK CONSTRAINT [FK_Song_Account]

ALTER TABLE [dbo].[Song]  WITH CHECK ADD  CONSTRAINT [FK_Song_Singer] FOREIGN KEY([Author])
REFERENCES [dbo].[Singer] ([ID])

ALTER TABLE [dbo].[Song] CHECK CONSTRAINT [FK_Song_Singer]

ALTER TABLE [dbo].[Song]  WITH CHECK ADD  CONSTRAINT [FK_Song_Singer] FOREIGN KEY([Author])
REFERENCES [dbo].[Singer] ([ID])

INSERT INTO Tag (TagName)
VALUES
    (N'Work'),
    (N'Personal'),
    (N'Family'),
    (N'Travel'),
    (N'Food')
