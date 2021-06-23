import Category from "./Category";
import Race from "./Race";
import Team from "./Team";

export default interface Season {
	id: number;
	name: string;
	last: Season;
	next: Season;
	categoryId: number;
	category: Category;
	races?: Race[];
	teams?: Team[];
}