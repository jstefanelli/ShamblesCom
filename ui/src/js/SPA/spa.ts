import Vue from 'vue';

export interface SPAData {
	redraw?: boolean,
	view?: string,
	data?: object
	title?: string,
	url: string
}

export type HttpMethod = "POST"|"GET"|"PATCH"|"PUT"|"DELETE";

export class SPA {
	private static CurrentPage: SPA = null;
	private static VueInstance: Vue = null;

	public static get Current() : SPA {
		return SPA.CurrentPage;
	}

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

	static navigate(targetUrl: string, method?: HttpMethod, requestData?: any, useJsonBody?: boolean) : Promise<SPA> {
		useJsonBody = useJsonBody === true;
		method = (typeof(method) == "string") ? method : "GET";
		let body: URLSearchParams = null;
		let jsonBody: string = null;
		let endUrl = targetUrl;

		if(requestData) {
			if(method == "GET" || method == "DELETE") {
				if (endUrl.indexOf("?") < 0) {
					endUrl += "?";
				} else {
					endUrl += "&";
				}

				let first = true;
				for(const [key, val] of Object.entries(requestData)) {
					if (!first)
						endUrl += "&";

					endUrl += encodeURIComponent(key) + "=" + encodeURIComponent(typeof(val) == "object" ? JSON.stringify(val) : String(val));
					first = false;
				}
			} else {
				if (!useJsonBody) {
					body = new URLSearchParams();
					for(const [k, v] of Object.entries(requestData)) {
						body.append(k, typeof(v) == "object" ? JSON.stringify(v) : String(v));
					}
				} else {
					jsonBody = JSON.stringify(requestData);
				}

			}
		}

		let saveUrl = endUrl;
		if (endUrl.indexOf("?") >= 0) {
			endUrl += ((endUrl.endsWith("?") ? "" : "&") + "spa=data");
		} else {
			endUrl += "?spa=data";
		}

		return new Promise<SPA>((resolve, reject) => {
			fetch(endUrl, {
				cache: "no-cache",
				method: method,
				body: useJsonBody ? jsonBody : body,
				headers: {
					"X-SPA-Data": "data",
					'Content-Type': useJsonBody ? "application/json" : "application/x-www-form-urlencoded"
				},
				redirect: "follow"
			}).then((response) => {

				if (!response.ok) {
					response.text().then((txt) => {
						let err = null;
						try {
							err = JSON.parse(txt);
						}catch(error) {
							console.warn("[SPA] Failed to parse error contents: ", error);
							err = response.statusText;
						}

						reject(err);
					}).catch((err) => {
						console.warn("[SPA] Failed to get error contents: ", err);
						reject(response.statusText);
					});
					return;
				}

				response.json().then((jsonData) => {
					let myData: SPAData = {
						url: jsonData.url ?? saveUrl,
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

	static navigateAndRender(targetUrl: string, method?: HttpMethod, requestData?: any, useJsonBody?: boolean) : Promise<void> {
		return new Promise((resolve, fail) => {
			SPA.navigate(targetUrl, method, requestData, useJsonBody).then(page => {
				 page.render();
				 resolve(null);
			}).catch((error) => {
				fail(error);
			});
		});
	}

	public render(pushState?: boolean) {
		pushState = (pushState === undefined) ? true : !!pushState;

		this.view = this.view != null ? this.view : ((SPA.CurrentPage) ? SPA.CurrentPage.view : "index" );

		let next = () => {

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
				if (pushState)
					window.history.pushState(state, this.title, url);
				else
					window.history.replaceState(state, this.title, url);
			}
		};

		if (this.redraw || !SPA.CurrentPage || SPA.CurrentPage.view != this.view) {
			
			if (SPA.VueInstance.$children[0] != null) {
				let mountedComponent = SPA.VueInstance.$children[0].$children[0] as any;
				if (mountedComponent !== undefined) {
					let unmountHandler = mountedComponent.unmounted;
					if (typeof(unmountHandler) == "function")
						unmountHandler();
				}
			}
			SPA.VueInstance.$children[0].$data.dynamicComponent = null;
			SPA.VueInstance.$nextTick(() => {
				let component = require("@/components/" + this.view).default;
				SPA.VueInstance.$children[0].$data.dynamicComponent = component;
				next();
			});
		}else {
			next();
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
		let query = window.location.search;
		let req = {} as any;
		let any = false;

		if (query && query.length > 0) {
			query = query.substring(1);
			
			let params = query.split('&');
			params.forEach(p => {
				let pair = p.split('=');
				if (pair[0] == 'spa')
					return;
				any = true;
				req[pair[0]] = pair[1];
			});
		}

		if (!path.startsWith("/")) {
			path = "/" + path;
		}

		SPA.navigate(path, "GET", any ? req : null).then((page) => {	
			page.render(false)
		});
	}
}

export default SPA;