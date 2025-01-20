function Setup() {

    //Getting the REST resources
    var RESTResources = $('#RESTResources').data('resources');

    // @todo: optimize this and refactor
    // @todo: reuse notyf everywhere
    var notyf = new Notyf({
        duration: 5000,
        ripple: false,
        position: { x: 'right', y: 'bottom' },
        dismissible: true,
        types: [
            {
                type: 'success',
                duration: 3000,
                dismissible: false,
            },
            {
                type: 'error',
                position: { x: 'right', y: 'bottom' },
                duration: 5000,
            }
        ]
    });

    // @todo: reuse
    function displayToastError(action, error, xhr) {
        var errorMessage = `<b>Could not ${action} this entry!</b><br>`;

        var errorData = xhr.responseJSON;
        if (errorData) {
            errorMessage += Object.keys(errorData).map(function (key) {
                var messagesWithDots = errorData[key].map(function (message) {
                    return '· ' + message;
                });
                return messagesWithDots.join('<br>');
            }).join('<br>');
        } else {
            errorMessage += `(${error})`
        }

        notyf.error(errorMessage);
    }

    $("#form").submit(function (e) {
        e.preventDefault();
        var formData = new FormData($('#form')[0]);

        var submitButtonId = e.originalEvent?.submitter?.id;

        if (submitButtonId == "article-save-and-publish") {
            $("#Article_IsPublished").attr("checked", true)
            $("#Article_IsPublished").val(true)

            formData.set('Article.IsPublished', true);

            if ($("#action-create").length) {
                submitButtonId = "action-create"
            } else {
                submitButtonId = "action-update"
            }
        }

        if (submitButtonId == "action-create") {
            $("#action-create").attr("disabled", "disabled")
            $.ajax({
                url: RESTResources.post,
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    notyf.success("Entry successfully created");
                    $("#action-create").attr('id', 'action-update');
                    $("#ObjectId").attr('value', response);
                    $("#action-update").removeAttr("disabled");
                },
                error: function (xhr, status, error) {
                    displayToastError("created", error, xhr);
                    $("#action-create").removeAttr("disabled");
                }
            })
            // Update
        } else if (submitButtonId == "action-update") {
            $("#action-update").attr("disabled", "disabled")
            $.ajax({
                url: RESTResources.put,
                type: 'PUT',
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    notyf.success("Entry updated successfully!");
                    $("#action-update").removeAttr("disabled");
                },
                error: function (xhr, status, error) {
                    displayToastError("update", error, xhr);
                    $("#action-update").removeAttr("disabled");
                }
            });
        }
        $("#action-update").removeAttr("disabled")
    });
}

$(function () {
    Setup();
});