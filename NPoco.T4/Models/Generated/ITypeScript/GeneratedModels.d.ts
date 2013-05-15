/**********  TypeScript Interfaces Generated from Models  **********/

interface ICompositeKeyObject {
	key1ID : number;
	key2ID : number;
	key3ID : number;
	textData? : string;
	dateEntered : any;
	dateUpdated? : any;
}

interface IIdentityObject {
	id : number;
	name? : string;
	age? : number;
	dateOfBirth? : any;
	savings? : any;
	dependentCount? : any;
	gender? : string;
	isActive : bool;
}

interface IKeyedGuidObject {
	id : any;
	name? : string;
	age? : number;
	dateOfBirth? : any;
	savings? : any;
	dependentCount? : any;
	gender? : string;
	isActive : bool;
}

interface IKeyedIntObject {
	id : number;
	name? : string;
	age? : number;
	dateOfBirth? : any;
	savings? : any;
	dependentCount? : any;
	gender? : string;
	isActive : bool;
}

interface IListObject {
	id : number;
	shortName : string;
	description : string;
	statusKey : string;
	sortOrder : number;
}

interface INoKeyNonDistinctObject {
	fullName : string;
	itemInt : number;
	optionalInt? : number;
	color? : string;
}

interface IObjectsWithCustomType {
	id : number;
	name? : string;
	mySpecialTypeField? : any;
}

