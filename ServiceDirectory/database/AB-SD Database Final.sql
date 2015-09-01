CREATE DATABASE ServiceDirectory
GO
USE ServiceDirectory
GO

SET DATEFORMAT DMY

----------Tài
--Organisation
--OrganisationReference
--OrganisationService
--OrganisationProgramme
--SupportingMaterial
--Directorate
--DEPARTMENT
--Team
--Premises
--Facility
--Volunteering
--MinorWork
--PremiseReference
--PremisesOpeningTime
--DetailPremises

--------------Tuan Anh
--Service
--ServicePremise
--Funding
--ServiceContract
--ServiceReference
--ReferenceData
--Programme
--Address
--Town
--County
--Country
--TrustRegion
--TrustDistrict
--GovOfficeRegion

---------------Minh Nhan
--Contact
--BusinessType
--ContacReference

---------------Quoc
--User
--Role


--------------------------Contact
CREATE TABLE tblContact
(
	ContactID int identity(1,1) primary key,
	FirstName nvarchar(50),
	Surname nvarchar(50),
	KnownAs nvarchar(50),
	OfficePhone nvarchar(20),
	MobilePhone nvarchar(20),
	StHomePhone nvarchar(20),
	Email nvarchar(50),
	ManagerId int foreign key references tblContact(ContactID),
	JobRole nvarchar(50),
	Workbase nvarchar(50),
	JobTitle nvarchar(50),
	IsActive bit ,
)

------------------------Country
CREATE TABLE tblCountry
(
	CountryID int identity(1,1) primary key,
	CountryName nvarchar(50),
	CountryDescription nvarchar(150),
)


------------------------County
CREATE TABLE tblCounty
(
	CountyID int identity(1,1) primary key,
	CountryID int foreign key references tblCountry(CountryID),
	CountyName nvarchar(50),
	CountyDescription nvarchar(150),
)

------------------------Town
CREATE TABLE tblTown
(
	TownID int identity(1,1) primary key,
	CountyID int foreign key references tblCounty(CountyID),
	TownName nvarchar(50),
	TownDescription nvarchar(150),
)

------------------------Address
CREATE TABLE tblAddress
(
	AddressID int identity(1,1) primary key,
	PostCode nvarchar(10),
	TownID int foreign key references tblTown(TownID),
	AddressDescription nvarchar(150),
	AddressName nvarchar(100),
)

-------------------------BusinessType
CREATE TABLE tblBusinessType
(
	BusinessID int identity(1,1) primary key,
	BusinessName nvarchar(200),
	SICCode int,
)

CREATE TABLE tblOrganisation
(
	OrgID		int identity(1,1) primary key not null,
	ContactID	int foreign key references tblContact(ContactID),
	AddressID	int foreign key references tblAddress(AddressID),
	BusinessID	int foreign key references tblBusinessType(BusinessID),
	OrgName		nvarchar(200),
	ShortDescription nvarchar(1000),
	FullDescription	nvarchar(2000),
	PhoneNumber	varchar(20),
	Fax	varchar(20),
	Email varchar(200),
	WebAddress	varchar(200),
	CharityNumber	int,
	CompanyNumber int,
	IsActive bit,
	AddressLine1	nvarchar(500),
	AddressLine2	nvarchar(500),
	AddressLine3	nvarchar(500),
	Preferred bit
)

----------------------ReferenceData
CREATE TABLE tblGroupReference
(
	GroupReferenceID int primary key,
	GroupValue nvarchar(100)
)

CREATE TABLE tblReferenceData
(
	RefID int identity(1,1) primary key,
	RefCode int not null foreign key references tblGroupReference(GroupReferenceID),
	RefValue nvarchar(50),
)

-------------------------ContactReference
CREATE TABLE tblContactReference
(
	ContactID int foreign key references tblContact(ContactID),
	RefID int foreign key references tblReferenceData(RefID),

	primary key (ContactID, RefID)
)


-------------------------OrganisationReference
CREATE TABLE tblOrganisationReference
(
	OrgID	int foreign key references tblOrganisation(OrgID),
	RefID	int foreign key references tblReferenceData(RefID),

	PRIMARY KEY (OrgID, RefID)
)

----------------------Programme
CREATE TABLE tblProgramme
(
	ProgrammeID int identity(1,1) primary key,
	ProgrammeName nvarchar(50) ,
	ContactID int foreign key references tblContact(ContactID),
	ProgrammeDescription nvarchar(150),
	IsActive bit ,
)

----------------Service
CREATE TABLE tblService
(
	ServiceID int identity(1,1) primary key,
	ProgrammeID int foreign key references tblProgramme(ProgrammeID),
	ContactID int foreign key references tblContact(ContactID),
	ServiceName nvarchar(50) ,
	ShortDescription nvarchar(100) ,
	ClientDescription nvarchar(150) NULL,
	StartExpected smalldatetime,
	StartDate smalldatetime,
	EndDate smalldatetime,
	ExtendableYears int,
	ExtendableMonths int,
	FullDescription nvarchar(150),
	DeptCode nvarchar(50) ,
	DescriptionDelivery nvarchar(150),
	ContractCode nvarchar(50),
	ContractValue nchar(10),
	ContractPayment bit,
	TimeLimitedYears smalldatetime,
	TimeLimitedMonths smalldatetime,
	IsActive bit ,
	Participation nvarchar(50),
)

-----------------------------------OrganisationService
CREATE TABLE tblOrganisationService
(
	OrgID	int foreign key references tblOrganisation(OrgID),
	ServiceID	int foreign key references tblService(ServiceID),
	Roles	varchar(100),

	PRIMARY KEY (OrgID, ServiceID)
)

-----------------------------------OrganisationProgramme
CREATE TABLE tblOrganisationProgramme
(
	OrgID	int foreign key references tblOrganisation(OrgID),
	ProgrammeID	int foreign key references tblProgramme(ProgrammeID),

	PRIMARY KEY (OrgID, ProgrammeID)
)

--------------------------Role
CREATE TABLE tblRole
(
	RoleID int primary key,
	RoleName nvarchar(50) not null,
	RoleDescription nvarchar(150)
)

-------------------------User
CREATE TABLE tblUser
(
	UserID int identity(1,1) primary key,
	Account nvarchar(50) ,
	UserPassword nvarchar(50) ,
	RoleID int foreign key references tblRole(RoleID),
	Email nvarchar(50) ,
	FullName nvarchar(50) 
)

------------------------------------SupportingMaterial
CREATE TABLE tblSupportingMaterial
(
	SupportID int identity(1,1) primary key,
	URL	varchar(200),
	OrgID	int foreign key references tblOrganisation(OrgID),
	UserID	int foreign key references tblUser(UserID),
	ShortDescription nvarchar(1000),
	TypeFile	varchar(100)	CHECK (TypeFile IN ('Doc', 'PDF', 'Excel')),
	AddedDate smalldatetime,
	IsActive bit
)


------------------------------------Directorate
CREATE TABLE tblDirectorate
(
	DirectorateID int identity(1,1) primary key not null,
	OrgID	int foreign key references tblOrganisation(OrgID),
	ContactID	int foreign key references tblContact(ContactID),
	AddressID	int foreign key references tblAddress(AddressID),
	BusinessID	int foreign key references tblBusinessType(BusinessID),
	DirectorateName nvarchar(200),
	ShortDescription nvarchar(1000),
	FullDescription	nvarchar(2000),
	AddressLine1	nvarchar(500),
	AddressLine2	nvarchar(500),
	AddressLine3	nvarchar(500),
	PhoneNumber	varchar(20),
	Fax	varchar(20),
	Email varchar(200),
	WebAddress	varchar(200),
	CharityNumber	int,
	CompanyNumber int,
	IsActive bit,
)


-------------------------------DEPARTMENT
CREATE TABLE tblDepartment
(
	DepartmentID int identity(1,1) primary key,
	DirectorateID int foreign key references tblDirectorate(DirectorateID),
	ContactID	int foreign key references tblContact(ContactID),
	AddressID	int foreign key references tblAddress(AddressID),
	BusinessID	int foreign key references tblBusinessType(BusinessID),
	DepartmentName nvarchar(200),
	ShortDescription nvarchar(1000),
	FullDescription	nvarchar(2000),
	AddressLine1	nvarchar(500),
	AddressLine2	nvarchar(500),
	AddressLine3	nvarchar(500),
	PhoneNumber	varchar(20),
	Fax	varchar(20),
	Email varchar(200),
	WebAddress	varchar(200),
	IsActive bit,
)

-----------------------------------Team
CREATE TABLE tblTeam
(
	TeamID int identity(1,1) primary key,
	DepartmentID int foreign key references tblDepartment(DepartmentID),
	ContactID	int foreign key references tblContact(ContactID),
	AddressID	int foreign key references tblAddress(AddressID),
	BusinessID	int foreign key references tblBusinessType(BusinessID),
	TeamName nvarchar(200),
	ShortDescription nvarchar(1000),
	FullDescription	nvarchar(2000),
	AddressLine1	nvarchar(500),
	AddressLine2	nvarchar(500),
	AddressLine3	nvarchar(500),
	PhoneNumber	varchar(20),
	Fax	varchar(20),
	Email varchar(200),
	WebAddress	varchar(200),
	IsActive bit,
)



----------------------Premises
CREATE TABLE tblPremises
(
	PremisesID int identity(1,1) primary key,
	PremisesName nvarchar(200),
	AddressID	int foreign key references tblAddress(AddressID),
	AddressLine1	nvarchar(500),
	LocationName nvarchar(200),
	KnowAs nvarchar(200),
	OrgID	int foreign key references tblOrganisation(OrgID),
	LocationStatus varchar(200) CHECK (LocationStatus in ('Pending Active', 'Active', 'Pending Closure', 'Closed items')),
	AddressLine2	nvarchar(500),
	AddressLine3	nvarchar(500),
	PrimaryLocation bit,
	LocationManaged bit,
	LocationDescription nvarchar(1000),
	PhoneNumber	varchar(20),
	Fax	varchar(20),
	MinicomNumber int,
	FlagDate smalldatetime,
	IsSpecialist bit,
	MediaContactID int foreign key references tblContact(ContactID),
	CateringContactID int foreign key references tblContact(ContactID),
	CateringType int foreign key references tblReferenceData(RefID),
	Network varchar(100) CHECK (Network in ('Open','Wip','Closed')),
	ClientITFacilities nvarchar(2000),
	LocalDemographicNotes nvarchar(1000),
	RoomAvailability bit,
	TravelDetails nvarchar(200),
	Bus nvarchar(200),
	Rail nvarchar(200),
	Airport nvarchar(200),
	HostingContactID int foreign key references tblContact(ContactID),
	ParkingSpaces int,
	ParkingAlternative varchar(200),
	RoomRate money,
	BBRate money,
	DBBRate money,
	DDRate money,
	TwentyfourhourRate money,
	TeaAndCoffee money,
	NoMeetingRoom money,
	MeetingRoomRate money,
	NegRoomRate money,
	BBNegRate money,
	DBBNegRate money,
	DDNegRate money,
	TwentyfourhourNegRate money,
	LastDate smalldatetime,
	ReDate smalldatetime,
	PreferredStatus varchar(100),
	Comment nvarchar(1000),
	Coding nvarchar(100)
)

-------------Facility
CREATE TABLE tblFacility
(
	FacilityID int identity(1,1) primary key,
	PremisesID int foreign key references tblPremises(PremisesID),
	FacilityType int foreign key references tblReferenceData(RefID),
	ShortDescription nvarchar(1000),
	RoomCapacity int,
	RoomSize int,
	ConnectivityType nvarchar(100),
	WireLess nvarchar(100),
	LeadContactID int foreign key references tblContact(ContactID),
	RoomHostID int foreign key references tblContact(ContactID),
	Notes nvarchar(1000),
	IsActive bit
)

-------------------Volunteering
CREATE TABLE tblVolunteering
(
	ContactID int foreign key references tblContact(ContactID),
	PremisesID int foreign key references tblPremises(PremisesID),
	Purpose nvarchar(1000),
	Detail nvarchar(2000),
	StartDate smalldatetime,
	EndDate smalldatetime,
	VolunteerNos int,
	IsActive bit,

	PRIMARY KEY (ContactID, PremisesID)
)

----------------------MinorWork
CREATE TABLE tblMinorWork
(
	MinorWorkID int identity(1,1) primary key,
	PremisesID int foreign key references tblPremises(PremisesID),
	ShortDecription nvarchar(1000),
	NoteAction nvarchar(1000),
	EstimatesCost  money,
	ActualCost money,
	DirectorateID int foreign key references tblDirectorate(DirectorateID),
	ContactID int foreign key references tblContact(ContactID),
	AuthorisedID int foreign key references tblContact(ContactID),
	Statu nvarchar(100),
	ReceiveDate smalldatetime,
	AuthorisedDate smalldatetime,
	StartDate smalldatetime,
	AnticipatedCompletion smalldatetime,
	CompletionDate smalldatetime,
	IsActive bit,
	IsProject bit
)


-------------------------PremiseReference
CREATE TABLE tblPremisesReference
(
	PremisesID int foreign key references tblPremises(PremisesID),
	RefID	int foreign key references tblReferenceData(RefID),

	PRIMARY KEY (PremisesID, RefID)
)
--------------------------PremisesOpeningTime
CREATE TABLE tblPremisesOpeningTime
(
	PremisesOpeningTimeID int identity(1,1) primary key,
	PremisesID int foreign key references tblPremises(PremisesID),
	WeekendDay varchar(50),
	StartTime datetime,
	EndTime datetime
)

------------------DetailPremises
CREATE TABLE tblDetailPremises
(
	PreID int foreign key references tblPremises(PremisesID),
	PreIDRelationShip int foreign key references tblPremises(PremisesID),

	PRIMARY KEY (PreID, PreIDRelationShip)
)

------------------Funding
CREATE TABLE tblFunding
(
	FundingID int identity(1,1) primary key,
	ServiceID int foreign key references tblService(ServiceID),
	ContactID int foreign key references tblContact(ContactID),
	FundingSource int,
	FundingAmount int ,
	FundingStart smalldatetime ,
	FundingEnd smalldatetime ,
	FundingNeeds int ,
	ContinuationAmount int ,
	ContinuationDetails nvarchar(50) ,
	FundraisingText nvarchar(100) ,
	FundraisingWhy nvarchar(100) ,
	FundraisingNeeds int ,
	FundraisingRequired smalldatetime ,
	FundraisingComplete bit ,
	CompletedDate smalldatetime ,
	DonorAnonymous bit ,
	DonorAmount int ,
	DonationDate smalldatetime ,
	DonationIncremental bit ,
)

-------------------ServicePremise
CREATE TABLE tblServicePremise
(
	ServiceID int foreign key references tblService(ServiceID),
	PremisesID int foreign key references tblPremises(PremisesID),
	ProjectCode varchar(20),

	primary key (ServiceID, PremisesID)
)

--------------------ServiceContract
CREATE TABLE tblServiceContract
(
	ServiceID int foreign key references tblService(ServiceID),
	RefID int foreign key references tblReferenceData(RefID),

	primary key (ServiceID, RefID)
)

----------------------ServiceReference
CREATE TABLE tblServiceReference
(
	ServiceID int foreign key references tblService(ServiceID),
	RefID int foreign key references tblReferenceData(RefID),

	primary key (ServiceID, RefID)
)


------------------------TrustRegion
CREATE TABLE tblTrustRegion
(
	TrustRegionID int identity(1,1) primary key,
	CountryID int foreign key references tblCountry(CountryID),
	TrustRegionName nvarchar(50) ,
	TrustRegionDescription nvarchar(150) ,
	IsActive bit,
)

------------------------TrustDistrict
CREATE TABLE tblTrustDistrict
(
	TrustDistrictID int identity(1,1) primary key,
	TrustRegionID int foreign key references tblTrustRegion(TrustRegionID),
	TrustDistrictName nvarchar(50) ,
	TrustDistrictDescription nvarchar(150) NULL,
	IsActive bit,
)

------------------------GovOfficeRegion
CREATE TABLE tblGovOfficeRegion
(
	GovOfficeRegionID int identity(1,1) primary key,
	CountyID int foreign key references tblCounty(CountyID),
	GovOfficeRegionName nvarchar(50) ,
	GovOfficeRegionDescription nvarchar(150),
	IsActive bit,
)



INSERT INTO tblRole (RoleID,RoleName,RoleDescription) values(1,'NormalUser','Normal user')
INSERT INTO tblRole (RoleID,RoleName,RoleDescription) values(2,'SuperUser','Super user')
INSERT INTO tblUser (RoleID,Account,UserPassword,Email, FullName) VALUES(1,'quoc','123456','quockhin@gmail.com', 'Quoc')
INSERT INTO tblUser (RoleID,Account,UserPassword,Email, FullName) VALUES(2,'tai','123456','uitdptai@gmail.com', 'Tai')


INSERT INTO tblGroupReference(GroupReferenceID, GroupValue) values
								(1, 'organisation specicalism'),
								(2, 'service personal circumstances capabilities'),
								(3, 'service disabilities capabilities'),
								(4, 'service ethnicity capabilities'),
								(5, 'service barriers capabilities'),
								(6, 'accreditation'),
								(7, 'service benefits capabilities');

								
INSERT INTO tblReferenceData (RefCode, RefValue) values
							( 1, 'Blind/Partially Sighted'),
							( 1, 'Deaf/Hard of Hearing'),
							(1, 'Dyslexia'),
							(1, 'Learning Disability'),
							(1, 'Mental Health'),

							(2, 'Carer Responsibilities'),
							(2, 'Lone Parent'),

							( 3, 'Chest, Breathing problems'),
							( 3, 'Condition restricting mobility'),
							( 3, 'Diabetes'),
							( 3, 'Difficulty in hearing'),

							( 4, 'White British'),
							( 4, 'White Irish'),
							( 4, 'Other White'),
							( 4, 'White & Black Caribbean'),
							( 4, 'White & Black African'),

							( 5, 'Lone Parent'),
							( 5, 'ESOL'),
							( 5, 'Refugee'),
							( 5, 'Basic Skills'),

							( 6, 'Two Ticks'),
							( 6, 'Investors In People'),
							( 6, 'ISO 9001'),
							( 6, 'ISO 14001'),
							( 6, 'ISO 27001'),

							( 7, 'Disability Living Allowance'),
							( 7, 'Employment'),
							( 7, 'Incapacity'),
							( 7, 'Income Support');



INSERT INTO tblCountry(CountryName) values (N'Việt Nam')
INSERT INTO tblCounty(CountryID, CountyName) values(1, N'Miền Tây' )
INSERT INTO tblTown(CountyID, TownName) values (1, N'Sóc Trăng')
INSERT INTO tblTown(CountyID, TownName) values (1, N'Cà Mau')
INSERT INTO tblAddress(TownID, PostCode) values(1, '970000')
INSERT INTO tblAddress(TownID, PostCode) values(2, '950000')

INSERT INTO tblBusinessType(SICCode, BusinessName) values
							(01160, 'abaca and other vegetable textile fibre growing'),
							(10110, 'abattoir (manufacture)'),
							(17120, 'abrasive base paper (manufacture)'),
							(23910, 'abrasive bonded disc, wheel and segment (manufacture)'),
							(23910, 'abrasive cloth (manufacture)'),
							(23910, 'abrasive grain (manufacture)'),
							(23910, 'abrasive grain of aluminium oxide (manufacture)'),
							(23910, 'abrasive grain of artificial corundum (manufacture)');

INSERT INTO tblOrganisation(OrgName, ShortDescription, BusinessID, AddressLine1, AddressID, PhoneNumber) values
							('Organisation name one', 'Short Description', 1, 'Address line one', 1, '1111111111'),
							('Organisation name two', 'Short Description', 2, 'Address line two', 1, '2222222222'),
							('Organisation name three', 'Short Description', 3, 'Address line three', 2, '3333333333');


INSERT INTO tblContact(FirstName, Surname, IsActive) VALUES 
					  ('Phat', 'Tai', '1'),
					  ('Cam', 'Quoc', '1');

INSERT INTO tblDirectorate(OrgID, ContactID, AddressID, BusinessID) values
						  (1, 1, 1, 2);

INSERT INTO tblDepartment( DepartmentName, DirectorateID) VALUES
						('Department one', 1);

INSERT INTO tblTeam (TeamName, DepartmentID, ContactID, AddressID, BusinessID) VALUES
					('Team one', 1, 1, 1, 1),
					('Team two', 1, 1, 1, 2),
					('Team three', 1, 2, 2, 3),
					('Team four', 1, 2, 1, 4);
