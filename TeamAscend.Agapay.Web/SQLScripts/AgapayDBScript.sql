USE AgapayTestDB


CREATE TABLE UserAccount (

[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[Username] [nvarchar](255) NOT NULL,
[Role] [nvarchar](255) NOT NULL,
[Password] [nvarchar](255) NOT NULL,
[FirstName] [nvarchar](255) NOT NULL,
[LastName] [nvarchar](255) NOT NULL,
[MiddleName] [nvarchar](255) NOT NULL,
[BirthDate] [nvarchar](255) NOT NULL,
[EmailAddress] [nvarchar](255) NOT NULL,
[ContactNo1] [nvarchar](255) NOT NULL,
[ContactNo2] [nvarchar](255) NULL,
[AddressLine1] [nvarchar](255) NOT NULL,
[AddressLine2] [nvarchar](255) NULL,
[City] [nvarchar](255) NOT NULL,
[Region] [nvarchar](255) NOT NULL,
[Barangay] [nvarchar](255) NULL,
[ZipPostCode] [nvarchar](255) NOT NULL,
[IsDeleted] [bit] NOT NULL,
[CreatedBy] [nvarchar](255) NULL,
[CreatedDate] [datetime] NULL,
[ModifiedBy] [nvarchar](255) NULL,
[ModifiedDate] [datetime] NULL,
)


CREATE TABLE GoPlan
(
[ID] [int] IDENTITY(1,1)  NOT NULL PRIMARY KEY,
[MeetingPointPIN] [nvarchar](255) NOT NULL,
[MeetingPointAddresss] [nvarchar](255) NOT NULL,
[PhoneNumber] [nvarchar](255) NOT NULL,
[FamilyMemberRole] [nvarchar](255) NOT NULL,
[IsDeleted] [bit] NOT NULL,
[CreatedBy] [nvarchar](255) NULL,
[CreatedDate] [datetime] NULL,
[ModifiedBy] [nvarchar](255) NULL,
[ModifiedDate] [datetime] NULL,
)


CREATE TABLE GoBag(
[ID] [int] IDENTITY(1,1)  NOT NULL PRIMARY KEY,
[GoPlanID] [int] NOT NULL,
[Category] [nvarchar](255) NOT NULL,
[Description] [nvarchar](255) NOT NULL,
[IsDeleted] [bit] NOT NULL,
[CreatedBy] [nvarchar](255) NULL,
[CreatedDate] [datetime] NULL,
[ModifiedBy] [nvarchar](255) NULL,
[ModifiedDate] [datetime] NULL,

)