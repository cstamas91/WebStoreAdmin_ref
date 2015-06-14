use ITStore;
GO

drop table Product;
drop table Category;
drop table Purchases;
GO

create table Category(
	ID int identity(1,1) primary key,
	Name varchar(40) not null
)
GO

create table Product(
	ID int identity(1,1) primary key,
	CategoryID int not null,
	Manufacturer varchar(40) not null,
	Model varchar(40) not null,
	Descr varchar(1000) not null,
	Price int not null,
	Stock int not null,
	IsActive bit not null,
	constraint ProductToCategory
	foreign key (CategoryID)
	references Category(ID)
)
GO

create table Purchases(
	Name varchar(40) not null,
	Addr varchar(40) not null,
	Phone varchar(20) not null,
	ID integer identity(1,1) primary key not null,
	Completion bit not null
)
GO

create table PurchaseToProduct(
	PurchaseID integer not null,
	ProductID integer not null,
	constraint ConnectionToProduct
	foreign key (ProductID) references Product(ID),
	constraint ConnectionToPurchases
	foreign key (PurchaseID) references Purchases(ID)
)
GO