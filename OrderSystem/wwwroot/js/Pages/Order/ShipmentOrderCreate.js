(function () {
    var vm = new Vue({
        el: '#CreateForm',
        data: {
            IsSending: false, // prevent double send
            Number: "",
            Type: "",
            DeliveryDate: "",
            FinishDate: "",
            Remarks: "",
            Address: "",
            SignName: "",
            Errors: {},
            ProductOption:[],
            OrderDetails: []
        },
        mounted: function () {
            this.ProductOption = $Page.ProductData;
        },
        computed: {
            Total: function () {
                var _total = 0;
                this.OrderDetails.forEach(item => {
                    if (item.ProductUnit && item.ProductId) {
                        _total += Number(item.ProductUnit) * Number(this.getProductPrice(item.ProductId))
                    }
                })

                return isNaN(_total) ? 0 : _total;
            }
        },
        methods: {
            getProductPrice(Id) {
                const target = this.ProductOption.find(x =>
                    Number(x.Id) === Number(Id))
                if (target) {
                    return target.Price
                } else {
                    return 0
                }
            },
            createRow() {
                this.OrderDetails.push({});
            },
            deleteRow(index) {
                this.OrderDetails.splice(index, 1);
            },
            createOrder() {
                if (this.IsSending === false) {
                    this.IsSending = true
                    $.ajax({
                        type: 'POST',
                        context: this,
                        url: '/Order/ShipmentOrderCreate',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data: {
                            Order: {
                                Number: this.Number,
                                Type: this.Type,
                                DeliveryDate: this.DeliveryDate,
                                FinishDate: this.FinishDate,
                                Total: this.Total,
                                Remarks: this.Remarks,
                                Address: this.Address,
                                SignName: this.SignName
                            },
                            OrderDetails: this.OrderDetails
                        },
                        success: function (res) {
                            if (res.isSuccess) {
                                this.Errors = {};
                                alert('成功建立訂單!')
                            } else {
                                this.Errors = res.error;
                                alert('訂單建立失敗，請查看錯誤')

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
    $(document).ready(function () {
        $("#DeliveryDate").datepicker().on("change", function (e) {
            vm.$data.DeliveryDate = $(this).val();
        });
        $("#FinishDate").datepicker().on("change", function (e) {
            vm.$data.FinishDate = $(this).val();
        });
    })

})()
