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
	KnownAs nvarchar(50) NULL,
	OfficePhone nvarchar(20) NULL,
	MobilePhone nvarchar(20) NULL,
	StHomePhone nvarchar(20) NULL,
	Email nvarchar(50) NULL,
	ManagerId uniqueidentifier foreign key references tblContact(ContactID),
	JobRole nvarchar(50) NULL,
	Workbase nvarchar(50) NULL,
	JobTitle nvarchar(50) NULL,
	IsActive bit NOT NULL,
)

------------------------Town
CREATE TABLE tblTown
(
	TownID uniqueidentifier primary key,
	ContactID uniqueidentifier foreign key references tblContact(ContactID),
	TownName nvarchar(50) NOT NULL,
	TownDescription nvarchar(150) NULL,
)

------------------------Address
CREATE TABLE tblAddress
(
	AddressID uniqueidentifier primary key,
	PostCode nvarchar(10) NOT NULL,
	TownID uniqueidentifier foreign key references tblTown(TownID),
	AddressDescription nvarchar(150) NULL,
	AddressName nvarchar(100) NULL,
)

-------------------------BusinessType
CREATE TABLE tblBusinessType
(
	BusinessID uniqueidentifier primary key,
	BusinessName nvarchar(50) not null,
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
	PhoneNumber	int,
	Fax	int,
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
CREATE TABLE tblReferenceData
(
	RefID uniqueidentifier primary key,
	RefCode int not null,
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
	ProgrammeDescription nvarchar(150) NULL,
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
	StartExpected smalldatetime NULL,
	StartDate smalldatetime NULL,
	EndDate smalldatetime NULL,
	ExtendableYears int NULL,
	ExtendableMonths int NULL,
	FullDescription nvarchar(150) NULL,
	DeptCode nvarchar(50) NOT NULL,
	DescriptionDelivery nvarchar(150) NULL,
	ContractCode nvarchar(50) NULL,
	ContractValue nchar(10) NULL,
	ContractPayment bit NULL,
	TimeLimitedYears smalldatetime NULL,
	TimeLimitedMonths smalldatetime NULL,
	IsActive bit NOT NULL,
	Participation nvarchar(50) NULL,
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
)

------------------------------------SupportingMaterial
CREATE TABLE tblSupportingMaterial
(
	URL	varchar(200),
	OrgID	uniqueidentifier foreign key references tblOrganisation(OrgID),
	UserID	uniqueidentifier foreign key references tblUser(UserID),
	ShortDescription nvarchar(1000),
	TypeFile	varchar(100)	CHECK (TypeFile IN ('Doc', 'PDF', 'Excel')),
	AddedDate smalldatetime,
	IsActive bit,

	PRIMARY KEY (URL, OrgID)
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
	PhoneNumber	int,
	Fax	int,
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
	PhoneNumber	int,
	Fax	int,
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
	PhoneNumber	int,
	Fax	int,
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
	PhoneNumber	int,
	Fax	int,
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
	FundingSource int NULL,
	FundingAmount int NOT NULL,
	FundingStart smalldatetime NULL,
	FundingEnd smalldatetime NULL,
	FundingNeeds int NULL,
	ContinuationAmount int NULL,
	ContinuationDetails nvarchar(50) NULL,
	FundraisingText nvarchar(100) NULL,
	FundraisingWhy nvarchar(100) NULL,
	FundraisingNeeds int NULL,
	FundraisingRequired smalldatetime NULL,
	FundraisingComplete bit NULL,
	CompletedDate smalldatetime NULL,
	DonorAnonymous bit NULL,
	DonorAmount int NULL,
	DonationDate smalldatetime NULL,
	DonationIncremental bit NULL,
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

------------------------Country
CREATE TABLE tblCountry
(
	CountryID uniqueidentifier primary key,
	CountryName nvarchar(50) NOT NULL,
	CountryDescription nvarchar(150) NULL,
)


------------------------County
CREATE TABLE tblCounty
(
	CountyID uniqueidentifier primary key,
	CountryID uniqueidentifier foreign key references tblCountry(CountryID),
	CountyName nvarchar(50) NOT NULL,
	CountyDescription nvarchar(150) NULL,
)


------------------------TrustRegion
CREATE TABLE tblTrustRegion
(
	TrustRegionID uniqueidentifier primary key,
	CountryID uniqueidentifier foreign key references tblCountry(CountryID),
	TrustRegionName nvarchar(50) NOT NULL,
	TrustRegionDescription nvarchar(150) NULL,
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


