<template>
	<base-page>
		<div class="w-full p-12 flex items-center">
			<div class="flex-grow" ></div>
			<my-section class="flex flex-col p-12 items-start">
				<span class="font-m mb-3 text-red-500" v-if="errors && errors.name">
					<b>Error:</b> {{ errors.name }}
				</span>
				<span class="font-m mb-3">Email:</span>
				<Myinput type="text" class="width-350" v-model="form.email" ></Myinput>
				<span class="font-m mb-3 mt-10 ">Password:</span>
				<Myinput type="password" class="width-350" v-model="form.password" ></Myinput>
				<div class="flex self-stretch">
					<div class="flex-grow" ></div>
					<Mybutton :main="true" class="mt-4">
						<spa-link target="/login" method="POST" :reqData="form" >Login</spa-link>
					</Mybutton>
				</div>
			</my-section>
			<div class="flex-grow"></div>
		</div>
	</base-page>
	
</template>

<script lang="ts">
import SpaLink from '@/SPA/SpaLink.vue';
import BasePage from "@/components/layout/basePage.vue";
import Section from "@/components/layout/section.vue";
import { Vue, Component, Prop } from 'vue-property-decorator';
import Myinput from '../layout/controls/myinput.vue';
import Mybutton from '../layout/controls/mybutton.vue';

export class LoginData {
	public email: string;
	public password: string;
}

@Component({
	components: {
		SpaLink,
		BasePage,
		"my-section": Section,
		Myinput,
		Mybutton
	}
})
export default class extends Vue {
	@Prop({default: null}) errors: any;

	form: LoginData = new LoginData();
}
</script>