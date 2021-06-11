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
		method = (typeof(method) == "string") ? method : "GET";
		let body = (method != "GET" && method != "DELETE") && requestData ? JSON.stringify(requestData) : null;
		let endUrl = targetUrl;

		if(requestData && (method == "GET" || method == "DELETE")) {
			if (endUrl.indexOf("?") < 0) {
				endUrl += "?";
			} else {
				endUrl += "&";
			}

			let first = true;
			requestData.forEach((val, key) => {
				if (!first)
					endUrl += "&";

				endUrl += encodeURIComponent(key) + "=" + encodeURIComponent(val);
				first = false;
			});
		}

		let saveUrl = endUrl;
		if (endUrl.indexOf("?") >= 0) {
			endUrl += ((endUrl.endsWith("?") ? "" : "&") + "spa=data");
		} else {
			endUrl += "?spa=data";
		}

		return new Promise<SPA>((resolve, reject) => {
			console.log("Target: ", endUrl);
			fetch(endUrl, {
				cache: "no-cache",
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
						url: saveUrl,
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

	public render(pushState?: boolean) {
		pushState = (pushState === undefined) ? true : !!pushState;

		this.view = this.view != null ? this.view : ((SPA.CurrentPage) ? SPA.CurrentPage.view : "index" );

		if (this.redraw || !SPA.CurrentPage || SPA.CurrentPage.view != this.view) {
			let component = require("@/components/" + this.view).default;
			SPA.VueInstance.$children[0].$data.dynamicComponent = component;
		}
		
		SPA.VueInstance.$children[0].$data.viewData = this.data;
		
		SPA.CurrentPage = this;
		if(window.history) {
			let state = {
				view: this.view,
				title: this.title,
				url: this.url,
				data: this.data
			};

			let safePath = this.url.startsWith("/") ? this.url : '/' + this.url;
			let url = window.location.origin + safePath;
			console.log("Pushing url: ", url);
			if (pushState)
				window.history.pushState(state, this.title, url);
			else
				window.history.replaceState(state, this.title, url);
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
				}).render(false);
			});
		}

		let path = window.location.pathname;

		if (!path.startsWith("/")) {
			path = "/" + path;
		}

		SPA.navigate(path).then((page) => {	
			page.render(false)
		});
	}
}

export default SPA;