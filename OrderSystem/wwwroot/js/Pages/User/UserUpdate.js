(function () {
    var vm = new Vue({
        el: '#Form',
        data: {
            IsSending: false,
            Account: "",
            RoleId: "",
            Id: "",
            Name: "",
            Email: "",
            RoleName:"",
            Roles: [],
            Errors: {},
        },
        mounted: function () {
            this.Id = $Page.User.Id
            this.Account = $Page.User.Account
            this.RoleId = $Page.User.RoleId
            this.Name = $Page.User.Name
            this.Email = $Page.User.Email
            this.RoleName = $Page.User.RoleName
            this.Roles = $Page.Roles
        },
        methods: {
            create() {
                if (this.IsSending === false) {
                    this.IsSending = true
                    $.ajax({
                        type: 'POST',
                        context: this,
                        url: '/User/UserUpdate',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data: {
                            Id : this.Id,
                            RoleId: this.RoleId,
                            Name:  this.Name,
                            Email: this.Email
                        },
                        success: function (res) {
                            if (res.IsSuccess) {
                                this.Errors = {};
                                alert('成功更新!')
                                location.reload();
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
