import Category from "./Category";

export default interface Season {
	id: number;
	uuid: string;
	name: string;
	last: Season;
	next: Season;
	category: Category;
}