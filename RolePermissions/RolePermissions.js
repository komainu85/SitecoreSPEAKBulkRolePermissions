define(["sitecore"], function (Sitecore) {
    var RolePermissions = Sitecore.Definitions.App.extend({

        filesUploaded: [],

        initialized: function () { },

        initialize: function () {
            $.ajax({
                url: "/api/sitecore/RolePermissions/GetAllRoles",
                type: "POST",
                context: this,
                success: function (data) {
                    var json = jQuery.parseJSON(data);

                    for (var i = 0; i < json.length; i++) {
                        var obj = json[i];
                        this.JsonDS.add(obj);
                    }
                }
            });
        },

        ApplyPermissions: function () {
            this.pi.viewModel.show();

            var selectedRole = this.Role.viewModel.selectedValue();
            var selectedItems = this.tvItems.viewModel.checkedItemIds();
            var selectedPermissions = this.GetSelectedPermissions();

            $.ajax({
                url: "/api/sitecore/RolePermissions/ApplyUserPermissions",
                type: "POST",
                data: { role: selectedRole, "selectedItems": selectedItems, "selectedPermissions": JSON.stringify(selectedPermissions) },
                context: this,
                success: function (data) {
                    if (data == "True") {
                        this.miMessages.addMessage("notification", { text: "Permissions applied successfully for " + selectedRole, actions: [], closable: true, temporary: true });
                    } else {
                        this.miMessages.addMessage("warning", "An error occured applying permissions for " + selectedRole + " , please try again");
                    }
                    this.pi.viewModel.hide();
                }
            });



        },

        GetSelectedPermissions: function () {

            var readAccess = $('input:radio[name=Read]:checked').val();
            var writeAccess = $('input:radio[name=Write]:checked').val();
            var renameAccess = $('input:radio[name=Rename]:checked').val();
            var createAccess = $('input:radio[name=Create]:checked').val();
            var deleteAccess = $('input:radio[name=Delete]:checked').val();
            var administerAccess = $('input:radio[name=Administer]:checked').val();
            var inheritance = $('input:radio[name=Inheritance]:checked').val();

            var permissions = {"permissions": [
                    {
                        "read": readAccess,
                        "write": writeAccess,
                        "rename": renameAccess,
                        "create": createAccess,
                        "delete": deleteAccess,
                        "admin": administerAccess,
                        "inheritance": inheritance
                    }
                ]
            };

            return permissions;

        },

    });

    return RolePermissions;
});