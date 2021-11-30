(function () {
    var vm = new Vue({
        el: '#Form',
        data: {
            IsSending:false,
            Role: {
                Name: ""
            },
            Permissions: [],
            Errors: {},
        },
        methods: {
            create() {
                if (this.IsSending === false) {
                    this.IsSending = true
                    $.ajax({
                        type: 'POST',
                        context: this,
                        url: '/Role/RoleCreate',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data:{
                            Role: this.Role,
                            Permissions: this.Permissions.map(it => ({
                                code: it
                            }))
                        },
                        success: function (res) {
                            if (res.IsSuccess) {
                                this.Errors = {};
                                alert('成功建立!')
                                window.history.back();
                            } else {
                                this.Errors = res.Error;
                                alert('建立失敗，請查看錯誤')
                            }
                        },
                        complete: function (data) {
                            this.IsSending = false;
                        }
                    })
                }
            }
        }
    })

})()
