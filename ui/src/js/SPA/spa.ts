import Vue from 'vue';

export interface SPAData {
	redraw?: boolean,
	view?: string,
	data?: object
	title?: string,
	url: string
}

export class SPA {
	private static CurrentPage: SPA = null;
	private static VueInstance: Vue = null;

	private data: object;
	private view: string;
	private redraw: boolean;
	private title: string;
	private url: string;

	constructor(spaData?: SPAData) {
		this.data = spaData ? spaData.data : null;
		this.view = (spaData && spaData.view) ? spaData.view : null;
		this.redraw = (spaData) ? spaData.redraw : false;
		this.title = (spaData) ? spaData.title : "";
		this.url = (spaData) ? spaData.url : "/";
	}

	static navigate(targetUrl: string, method?: "POST"|"GET"|"PATCH"|"PUT"|"DELETE", requestData?: Map<string, any>) : Promise<SPA> {
		let body = (method != "GET" && method != "DELETE") && requestData ? JSON.stringify(requestData) : null; 
		method = (typeof(method) == "string") ? method : "GET";

		return new Promise<SPA>((resolve, reject) => {
			fetch(targetUrl, {
				method: method,
				body: body,
				headers: {
					"X-SPA-Data": "data"
				}
			}).then((response) => {
				if (!response.ok) {
					reject(response.statusText);
					return;
				}

				response.json().then((jsonData) => {
					let myData: SPAData = {
						url: targetUrl,
						redraw: (typeof(jsonData.redraw) == 'boolean') ? jsonData.redraw : false,
						view: (typeof(jsonData.view) == 'string') ? jsonData.view : null,
						data: jsonData.data,
						title: (typeof(jsonData.title) == 'string') ? jsonData.title : document.title
					};

					resolve(new SPA(myData));
				}).catch(reject);
			}).catch(reject);

			//TODO: Custom "reject" handlers?
		});
	}

	public render() {
		//TODO: Render page

		this.view = this.view != null ? this.view : ((SPA.CurrentPage) ? SPA.CurrentPage.view : "index" );

		if (this.redraw || !SPA.CurrentPage || SPA.CurrentPage.view != this.view) {
			let component = require("@/components/" + this.view).default;
			SPA.VueInstance.$children[0].$data.dynamicComponent = component;
		}
		
		SPA.VueInstance.$children[0].$data.viewData = this.data;
		
		SPA.CurrentPage = this;
		if(window.history) {
			window.history.pushState({
				view: this.view,
				title: this.title,
				url: this.url,
				data: this.data
			}, this.title, this.url);
		}
	}

	static init() {
		if (!SPA.VueInstance) {
			let component = require("@/components/dynamic").default;
			SPA.VueInstance = new Vue({
				render: (h) => {
					return h(component, {
						props: {
							viewData: {},
							dynamicComponent: null
						}
					});
				}
			});
			SPA.VueInstance.$mount(document.getElementById("app"));
			window.addEventListener("popstate", (event) => {
				new SPA({
					url: event.state.url,
					data: event.state.data,
					title: event.state.title,
					redraw: true,
					view: event.state.view
				}).render();
			});
		}

		let path = window.location.pathname;

		if (!path.startsWith("/")) {
			path = "/" + path;
		}

		SPA.navigate(path).then((page) => {	
			page.render()
		});
	}
}

export default SPA;