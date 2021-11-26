Vue.component('error-message', {
  props: ['errors'],
  template: `
    <span>
         <div class="error-message" v-for="(value, key) in errors">{{ value }}</div>
    </span>
  `
})

