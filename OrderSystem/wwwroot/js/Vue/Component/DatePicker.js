Vue.component("date-picker", {
    template: "<input/>",
    props: ["dateFormat", "value"],
    watch: {
        value(value) {
            $(this.$el).datepicker({
                dateFormat: this.dateFormat,
                onSelect: function (date) {
                    self.$emit("input", date);
                }
            })
            if (!(this.value == null || this.value == '')) {
                $(this.$el).datepicker("setDate", new Date(this.value));
            }
        },
        dateFormat(dateFormat) {
            $(this.$el).datepicker("option", "dateFormat", dateFormat);
        }
    },
    mounted: function () {
        var self = this;
        $(this.$el).datepicker({
            dateFormat: this.dateFormat,
            onSelect: function (date) {
                self.$emit("input", date);
            }
        })
        if (!(this.value === null || this.value === '')) {
            $(this.$el).datepicker("setDate", new Date(this.value));
        } 
    },
    beforeDestroy: function () {
        $(this.$el)
            .datepicker("hide")
            .datepicker("destroy");
    }
});
