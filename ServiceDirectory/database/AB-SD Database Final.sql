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
	ContactID uniqueidentifier primary key,
	FirstName nvarchar(50) NOT NULL,
	Surname nvarchar(50) NOT NULL,
	KnownAs nvarchar(50),
	OfficePhone nvarchar(20),
	MobilePhone nvarchar(20),
	StHomePhone nvarchar(20),
	Email nvarchar(50),
	ManagerId uniqueidentifier foreign key references tblContact(ContactID),
	JobRole nvarchar(50),
	Workbase nvarchar(50),
	JobTitle nvarchar(50),
	IsActive bit NOT NULL,
)

------------------------Country
CREATE TABLE tblCountry
(
	CountryID uniqueidentifier primary key,
	CountryName nvarchar(50) NOT NULL,
	CountryDescription nvarchar(150),
)


------------------------County
CREATE TABLE tblCounty
(
	CountyID uniqueidentifier primary key,
	CountryID uniqueidentifier foreign key references tblCountry(CountryID),
	CountyName nvarchar(50) NOT NULL,
	CountyDescription nvarchar(150),
)

------------------------Town
CREATE TABLE tblTown
(
	TownID uniqueidentifier primary key,
	CountyID uniqueidentifier foreign key references tblCounty(CountyID),
	TownName nvarchar(50) NOT NULL,
	TownDescription nvarchar(150),
)

------------------------Address
CREATE TABLE tblAddress
(
	AddressID uniqueidentifier primary key,
	PostCode nvarchar(10) NOT NULL,
	TownID uniqueidentifier foreign key references tblTown(TownID),
	AddressDescription nvarchar(150),
	AddressName nvarchar(100),
)

-------------------------BusinessType
CREATE TABLE tblBusinessType
(
	BusinessID uniqueidentifier primary key,
	BusinessName nvarchar(200) not null,
	SICCode int not null,
)

CREATE TABLE tblOrganisation
(
	OrgID		uniqueidentifier primary key not null,
	ContactID	uniqueidentifier foreign key references tblContact(ContactID),
	AddressID	uniqueidentifier foreign key references tblAddress(AddressID),
	BusinessID	uniqueidentifier foreign key references tblBusinessType(BusinessID),
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
	RefID uniqueidentifier primary key,
	RefCode int not null foreign key references tblGroupReference(GroupReferenceID),
	RefValue nvarchar(50) not null,
)

-------------------------ContactReference
CREATE TABLE tblContactReference
(
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	RefID uniqueidentifier foreign key references tblReferenceData(RefID),

	primary key (ContactID, RefID)
)


-------------------------OrganisationReference
CREATE TABLE tblOrganisationReference
(
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
	RefID	uniqueidentifier foreign key references tblReferenceData(RefID),

	PRIMARY KEY (OrgID, RefID)
)

----------------------Programme
CREATE TABLE tblProgramme
(
	ProgrammeID uniqueidentifier primary key,
	ProgrammeName nvarchar(50) NOT NULL,
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	ProgrammeDescription nvarchar(150),
	IsActive bit NOT NULL,
)

----------------Service
CREATE TABLE tblService
(
	ServiceID uniqueidentifier primary key,
	ProgrammeID uniqueidentifier foreign key references tblProgramme(ProgrammeID),
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	ServiceName nvarchar(50) NOT NULL,
	ShortDescription nvarchar(100) NOT NULL,
	ClientDescription nvarchar(150) NULL,
	StartExpected smalldatetime,
	StartDate smalldatetime,
	EndDate smalldatetime,
	ExtendableYears int,
	ExtendableMonths int,
	FullDescription nvarchar(150),
	DeptCode nvarchar(50) NOT NULL,
	DescriptionDelivery nvarchar(150),
	ContractCode nvarchar(50),
	ContractValue nchar(10),
	ContractPayment bit,
	TimeLimitedYears smalldatetime,
	TimeLimitedMonths smalldatetime,
	IsActive bit NOT NULL,
	Participation nvarchar(50),
)

-----------------------------------OrganisationService
CREATE TABLE tblOrganisationService
(
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
	ServiceID	uniqueidentifier foreign key references tblService(ServiceID),
	Roles	varchar(100),

	PRIMARY KEY (OrgID, ServiceID)
)

-----------------------------------OrganisationProgramme
CREATE TABLE tblOrganisationProgramme
(
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
	ProgrammeID	uniqueidentifier foreign key references tblProgramme(ProgrammeID),

	PRIMARY KEY (OrgID, ProgrammeID)
)

--------------------------Role
CREATE TABLE tblRole
(
	RoleID uniqueidentifier primary key,
	RoleName nvarchar(50) not null,
	RoleDescription nvarchar(150)
)

-------------------------User
CREATE TABLE tblUser
(
	UserID uniqueidentifier primary key,
	Account nvarchar(50) NOT NULL,
	UserPassword nvarchar(50) NOT NULL,
	RoleID uniqueidentifier foreign key references tblRole(RoleID),
	Email nvarchar(50) NOT NULL,
	FullName nvarchar(50) NOT NULL
)

------------------------------------SupportingMaterial
CREATE TABLE tblSupportingMaterial
(
	SupportID uniqueidentifier primary key,
	URL	varchar(200),
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
	UserID	uniqueidentifier foreign key references tblUser(UserID),
	ShortDescription nvarchar(1000),
	TypeFile	varchar(100)	CHECK (TypeFile IN ('Doc', 'PDF', 'Excel')),
	AddedDate smalldatetime,
	IsActive bit
)


------------------------------------Directorate
CREATE TABLE tblDirectorate
(
	DirectorateID uniqueidentifier primary key not null,
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
	ContactID	uniqueidentifier foreign key references tblContact(ContactID),
	AddressID	uniqueidentifier foreign key references tblAddress(AddressID),
	BusinessID	uniqueidentifier foreign key references tblBusinessType(BusinessID),
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
	DepartmentID uniqueidentifier primary key,
	DirectorateID uniqueidentifier foreign key references tblDirectorate(DirectorateID),
	ContactID	uniqueidentifier foreign key references tblContact(ContactID),
	AddressID	uniqueidentifier foreign key references tblAddress(AddressID),
	BusinessID	uniqueidentifier foreign key references tblBusinessType(BusinessID),
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
	TeamID uniqueidentifier primary key,
	DepartmentID uniqueidentifier foreign key references tblDepartment(DepartmentID),
	ContactID	uniqueidentifier foreign key references tblContact(ContactID),
	AddressID	uniqueidentifier foreign key references tblAddress(AddressID),
	BusinessID	uniqueidentifier foreign key references tblBusinessType(BusinessID),
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
	PremisesID uniqueidentifier primary key,
	PremisesName nvarchar(200),
	AddressID	uniqueidentifier foreign key references tblAddress(AddressID),
	AddressLine1	nvarchar(500),
	LocationName nvarchar(200),
	KnowAs nvarchar(200),
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
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
	MediaContactID uniqueidentifier foreign key references tblContact(ContactID),
	CateringContactID uniqueidentifier foreign key references tblContact(ContactID),
	CateringType uniqueidentifier foreign key references tblReferenceData(RefID),
	Network varchar(100) CHECK (Network in ('Open','Wip','Closed')),
	ClientITFacilities nvarchar(2000),
	LocalDemographicNotes nvarchar(1000),
	RoomAvailability bit,
	TravelDetails nvarchar(200),
	Bus nvarchar(200),
	Rail nvarchar(200),
	Airport nvarchar(200),
	HostingContactID uniqueidentifier foreign key references tblContact(ContactID),
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
	FacilityID uniqueidentifier primary key,
	PremisesID uniqueidentifier foreign key references tblPremises(PremisesID),
	FacilityType uniqueidentifier foreign key references tblReferenceData(RefID),
	ShortDescription nvarchar(1000),
	RoomCapacity int,
	RoomSize int,
	ConnectivityType nvarchar(100),
	WireLess nvarchar(100),
	LeadContactID uniqueidentifier foreign key references tblContact(ContactID),
	RoomHostID uniqueidentifier foreign key references tblContact(ContactID),
	Notes nvarchar(1000),
	IsActive bit
)

-------------------Volunteering
CREATE TABLE tblVolunteering
(
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	PremisesID uniqueidentifier foreign key references tblPremises(PremisesID),
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
	MinorWorkID uniqueidentifier primary key,
	PremisesID uniqueidentifier foreign key references tblPremises(PremisesID),
	ShortDecription nvarchar(1000),
	NoteAction nvarchar(1000),
	EstimatesCost  money,
	ActualCost money,
	DirectorateID uniqueidentifier foreign key references tblDirectorate(DirectorateID),
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	AuthorisedID uniqueidentifier foreign key references tblContact(ContactID),
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
	PremisesID uniqueidentifier foreign key references tblPremises(PremisesID),
	RefID	uniqueidentifier foreign key references tblReferenceData(RefID),

	PRIMARY KEY (PremisesID, RefID)
)
--------------------------PremisesOpeningTime
CREATE TABLE tblPremisesOpeningTime
(
	PremisesOpeningTimeID uniqueidentifier primary key,
	PremisesID uniqueidentifier foreign key references tblPremises(PremisesID),
	WeekendDay varchar(50),
	StartTime datetime,
	EndTime datetime
)

------------------DetailPremises
CREATE TABLE tblDetailPremises
(
	PreID uniqueidentifier foreign key references tblPremises(PremisesID),
	PreIDRelationShip uniqueidentifier foreign key references tblPremises(PremisesID),

	PRIMARY KEY (PreID, PreIDRelationShip)
)

------------------Funding
CREATE TABLE tblFunding
(
	FundingID uniqueidentifier primary key,
	ServiceID uniqueidentifier foreign key references tblService(ServiceID),
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	FundingSource int,
	FundingAmount int NOT NULL,
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
	ServiceID uniqueidentifier foreign key references tblService(ServiceID),
	PremisesID uniqueidentifier foreign key references tblPremises(PremisesID),
	ProjectCode varchar(20),

	primary key (ServiceID, PremisesID)
)

--------------------ServiceContract
CREATE TABLE tblServiceContract
(
	ServiceID uniqueidentifier foreign key references tblService(ServiceID),
	RefID uniqueidentifier foreign key references tblReferenceData(RefID),

	primary key (ServiceID, RefID)
)

----------------------ServiceReference
CREATE TABLE tblServiceReference
(
	ServiceID uniqueidentifier foreign key references tblService(ServiceID),
	RefID uniqueidentifier foreign key references tblReferenceData(RefID),

	primary key (ServiceID, RefID)
)


------------------------TrustRegion
CREATE TABLE tblTrustRegion
(
	TrustRegionID uniqueidentifier primary key,
	CountryID uniqueidentifier foreign key references tblCountry(CountryID),
	TrustRegionName nvarchar(50) NOT NULL,
	TrustRegionDescription nvarchar(150) ,
	IsActive bit not null,
)

------------------------TrustDistrict
CREATE TABLE tblTrustDistrict
(
	TrustDistrictID uniqueidentifier primary key,
	TrustRegionID uniqueidentifier foreign key references tblTrustRegion(TrustRegionID),
	TrustDistrictName nvarchar(50) NOT NULL,
	TrustDistrictDescription nvarchar(150) NULL,
	IsActive bit not null,
)

------------------------GovOfficeRegion
CREATE TABLE tblGovOfficeRegion
(
	GovOfficeRegionID uniqueidentifier primary key,
	CountyID uniqueidentifier foreign key references tblCounty(CountyID),
	GovOfficeRegionName nvarchar(50) NOT NULL,
	GovOfficeRegionDescription nvarchar(150) NULL,
	IsActive bit not null,
)



INSERT INTO tblRole (RoleID,RoleName,RoleDescription) values(Convert(uniqueidentifier, '18ad571d-2c89-4b2d-b2a3-088d9518b2f2'),'NormalUser','Normal user')
INSERT INTO tblRole (RoleID,RoleName,RoleDescription) values(Convert(uniqueidentifier, '80d2e54c-638f-4b6c-989f-392002f1b211'),'SuperUser','Super user')
INSERT INTO tblUser (UserID,RoleID,Account,UserPassword,Email, FullName) VALUES(NEWID(),'18ad571d-2c89-4b2d-b2a3-088d9518b2f2','quoc','123456','quockhin@gmail.com', 'Quoc')
INSERT INTO tblUser (UserID,RoleID,Account,UserPassword,Email, FullName) VALUES(NEWID(),'80d2e54c-638f-4b6c-989f-392002f1b211','tai','123456','uitdptai@gmail.com', 'Tai')


INSERT INTO tblGroupReference(GroupReferenceID, GroupValue) values
								(1, 'organisation specicalism'),
								(2, 'service personal circumstances capabilities'),
								(3, 'service disabilities capabilities'),
								(4, 'service ethnicity capabilities'),
								(5, 'service barriers capabilities'),
								(6, 'accreditation'),
								(7, 'service benefits capabilities');

								
INSERT INTO tblReferenceData (RefID, RefCode, RefValue) values
							(NEWID(), 1, 'Blind/Partially Sighted'),
							(NEWID(), 1, 'Deaf/Hard of Hearing'),
							(NEWID(), 1, 'Dyslexia'),
							(NEWID(), 1, 'Learning Disability'),
							(NEWID(), 1, 'Mental Health'),

							(NEWID(), 2, 'Carer Responsibilities'),
							(NEWID(), 2, 'Lone Parent'),

							(NEWID(), 3, 'Chest, Breathing problems'),
							(NEWID(), 3, 'Condition restricting mobility'),
							(NEWID(), 3, 'Diabetes'),
							(NEWID(), 3, 'Difficulty in hearing'),

							(NEWID(), 4, 'White British'),
							(NEWID(), 4, 'White Irish'),
							(NEWID(), 4, 'Other White'),
							(NEWID(), 4, 'White & Black Caribbean'),
							(NEWID(), 4, 'White & Black African'),

							(NEWID(), 5, 'Lone Parent'),
							(NEWID(), 5, 'ESOL'),
							(NEWID(), 5, 'Refugee'),
							(NEWID(), 5, 'Basic Skills'),

							(NEWID(), 6, 'Two Ticks'),
							(NEWID(), 6, 'Investors In People'),
							(NEWID(), 6, 'ISO 9001'),
							(NEWID(), 6, 'ISO 14001'),
							(NEWID(), 6, 'ISO 27001'),

							(NEWID(), 7, 'Disability Living Allowance'),
							(NEWID(), 7, 'Employment'),
							(NEWID(), 7, 'Incapacity'),
							(NEWID(), 7, 'Income Support');



INSERT INTO tblCountry(CountryID, CountryName) values (CONVERT(uniqueidentifier, '9cf02af4-7c47-4225-afa8-d3af64a8dd02'), N'Việt Nam')
INSERT INTO tblCounty(CountyID, CountryID, CountyName) values(CONVERT(uniqueidentifier, '86a752c0-e2e7-4829-9127-00bd118ce194'),'9cf02af4-7c47-4225-afa8-d3af64a8dd02', N'Miền Tây' )
INSERT INTO tblTown(TownID, CountyID, TownName) values (CONVERT(uniqueidentifier, '7af60496-f170-40c3-88ab-e090aa8d4af5'), '86a752c0-e2e7-4829-9127-00bd118ce194', N'Sóc Trăng')
INSERT INTO tblTown(TownID, CountyID, TownName) values (CONVERT(uniqueidentifier, '4db27d47-2de5-4202-a48b-71c1ab2cc9d8'), '86a752c0-e2e7-4829-9127-00bd118ce194', N'Cà Mau')
INSERT INTO tblAddress(AddressID, TownID, PostCode) values(CONVERT(uniqueidentifier, '5d109451-6738-4def-96d2-5a9e96783564'), '4db27d47-2de5-4202-a48b-71c1ab2cc9d8', '970000')
INSERT INTO tblAddress(AddressID, TownID, PostCode) values(CONVERT(uniqueidentifier, '692377b4-9291-4b85-ad3f-6e24dd663c4d'), '7af60496-f170-40c3-88ab-e090aa8d4af5', '950000')

INSERT INTO tblBusinessType(BusinessID, SICCode, BusinessName) values
							(CONVERT(uniqueidentifier, '6da12942-7dd3-4416-8c75-2d96043790c3'), 01160, 'abaca and other vegetable textile fibre growing'),
							(CONVERT(uniqueidentifier, 'fb077524-0ab3-4a10-abf6-467005e9c637'), 10110, 'abattoir (manufacture)'),
							(convert(uniqueidentifier, '2da6aa37-5da4-49c3-aeaf-a7bec749fbeb'), 17120, 'abrasive base paper (manufacture)'),
							(CONVERT(uniqueidentifier, '29e20707-b0b3-47ca-b183-b4d1a5e9bfe8'), 23910, 'abrasive bonded disc, wheel and segment (manufacture)'),
							(convert(uniqueidentifier, '61fe44a1-752f-497d-aade-66027ff912ac'), 23910, 'abrasive cloth (manufacture)'),
							(CONVERT(uniqueidentifier, 'f5929bae-8e75-4ab1-9fde-8580c744b224'), 23910, 'abrasive grain (manufacture)'),
							(CONVERT(uniqueidentifier, '31dcc354-cd1c-4766-a14b-7db008e1e16e'), 23910, 'abrasive grain of aluminium oxide (manufacture)'),
							(CONVERT(uniqueidentifier, 'ac3d7148-f1ab-420c-b03c-939e0f92cb43'), 23910, 'abrasive grain of artificial corundum (manufacture)');

INSERT INTO tblOrganisation(OrgID, OrgName, ShortDescription, BusinessID, AddressLine1, AddressID, PhoneNumber) values
							(CONVERT(uniqueidentifier, 'd5f1ffe9-cd05-4e24-9cd0-41d8812980ea'), 'Organisation name one', 'Short Description', '6da12942-7dd3-4416-8c75-2d96043790c3', 'Address line one', '5d109451-6738-4def-96d2-5a9e96783564', '1111111111'),
							(CONVERT(uniqueidentifier, 'e66406b7-68d7-467e-a7ae-fad426f925e9'), 'Organisation name two', 'Short Description', 'fb077524-0ab3-4a10-abf6-467005e9c637', 'Address line two', '5d109451-6738-4def-96d2-5a9e96783564', '2222222222'),
							(CONVERT(uniqueidentifier, 'fce3c330-2369-48bf-9163-f3c42a9a74d3'), 'Organisation name three', 'Short Description', '2da6aa37-5da4-49c3-aeaf-a7bec749fbeb', 'Address line three', '5d109451-6738-4def-96d2-5a9e96783564', '3333333333');


INSERT INTO tblContact(ContactID, FirstName, Surname, IsActive) VALUES 
					  (CONVERT(uniqueidentifier, 'a38c8ba8-883c-450b-a7d9-521e2853c9ee'), 'Phat', 'Tai', '1'),
					  (CONVERT(uniqueidentifier, 'c3b2060f-0df6-474d-89c8-5b654536d78d'), 'Cam', 'Quoc', '1');

INSERT INTO tblDirectorate(DirectorateID, OrgID, ContactID, AddressID, BusinessID) values
						  (CONVERT(uniqueidentifier, '60e1dece-593e-4f36-b32e-2f10dfc296fe'), 'd5f1ffe9-cd05-4e24-9cd0-41d8812980ea', 'a38c8ba8-883c-450b-a7d9-521e2853c9ee', '5d109451-6738-4def-96d2-5a9e96783564', '6da12942-7dd3-4416-8c75-2d96043790c3');

INSERT INTO tblDepartment( DepartmentID, DepartmentName, DirectorateID) VALUES
						(CONVERT(uniqueidentifier, 'b01bdb82-ed9c-44ab-8bed-49b5868417f3'), 'Department one', '60e1dece-593e-4f36-b32e-2f10dfc296fe');

INSERT INTO tblTeam (TeamID, TeamName, DepartmentID, ContactID, AddressID, BusinessID) VALUES
					(CONVERT(uniqueidentifier, '564b581f-7626-44ce-b126-26273859cb94'), 'Team one', 'b01bdb82-ed9c-44ab-8bed-49b5868417f3', 'a38c8ba8-883c-450b-a7d9-521e2853c9ee', '5d109451-6738-4def-96d2-5a9e96783564', '6da12942-7dd3-4416-8c75-2d96043790c3'),
					(CONVERT(uniqueidentifier, '514729bb-9da6-46ac-b4b1-6df191e59c0a'), 'Team two', 'b01bdb82-ed9c-44ab-8bed-49b5868417f3', 'a38c8ba8-883c-450b-a7d9-521e2853c9ee', '5d109451-6738-4def-96d2-5a9e96783564', 'fb077524-0ab3-4a10-abf6-467005e9c637'),
					(CONVERT(uniqueidentifier, '0651a43f-177e-4d43-950f-d82984edbc6d'), 'Team three', 'b01bdb82-ed9c-44ab-8bed-49b5868417f3', 'c3b2060f-0df6-474d-89c8-5b654536d78d', '5d109451-6738-4def-96d2-5a9e96783564', '61fe44a1-752f-497d-aade-66027ff912ac'),
					(CONVERT(uniqueidentifier, 'fc5b2c28-3f65-4bfb-a22a-0f9a1c7e788b'), 'Team four', 'b01bdb82-ed9c-44ab-8bed-49b5868417f3', 'c3b2060f-0df6-474d-89c8-5b654536d78d', '692377b4-9291-4b85-ad3f-6e24dd663c4d', '31dcc354-cd1c-4766-a14b-7db008e1e16e');
