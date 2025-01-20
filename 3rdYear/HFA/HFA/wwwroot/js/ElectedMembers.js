$(document).ready(function () {
    let copyElements = function (clonedNode) {
        let numberOfMembers = $("#MembersContainer .GroupMembers").toArray().length;
        clonedNode.removeClass("hidden");
        clonedNode.removeClass("GroupMembersToCopy");
        clonedNode.addClass("GroupMembers");
        let clonedElements = clonedNode.find("input");
        clonedElements.each(function (index, element) {
            $(element).attr("name", "Members[" + numberOfMembers + "]." + element.id)
        });
    };

    let renumberElements = function () {
        let containerElements = $("#MembersContainer .GroupMembers").toArray();
        containerElements.forEach((element, index) => {
            let elementInputs = $(element).find("input").toArray();
            elementInputs.forEach(input => {
                $(input).removeAttr("name");
                $(input).attr("name", "Members[" + index + "]." + input.id);
            })
        })
    }

    $("#MembersContainer").on('click', '.removeMemberBtn', function () {
        $(this).parent().parent().remove();
        renumberElements();
    });

    $("#AddMemberBtn").on('click', function () {
        let clonedNode = $(".GroupMembersToCopy").clone();
        copyElements(clonedNode);
        $("#MembersContainer").append(clonedNode);
    });

    $("#form").on('submit', function () {
        let membersInputs = $("#MembersContainer .GroupMembers").toArray();
        membersInputs.forEach(function (element) {
            let inputs = $(element).find("input#MemberFirstName, input#MemberLastName").toArray();
            if ((inputs[0].value == null || inputs[0].value.trim() == "") && (inputs[1].value == null || inputs[1].value.trim() == "")) {
                $(inputs).parent().parent().parent().remove();
                //console.log($(inputs).parent().parent())
            }
        });
        renumberElements();
    });

})