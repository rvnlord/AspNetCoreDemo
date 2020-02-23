if (window.location.hash === "#_=_") {
    window.location.hash = "";
    history.pushState("", document.title, window.location.pathname);
    e.preventDefault();
}

function confirmDelete(uniqueId, isDeleteClicked) {
    const deleteSpan = `deleteSpan_${uniqueId}`;
    const confirmDeleteSpan = `confirmDeleteSpan_${uniqueId}`;

    if (isDeleteClicked) {
        $(`#${deleteSpan}`).hide();
        $(`#${confirmDeleteSpan}`).show();
    } else {
        $(`#${deleteSpan}`).show();
        $(`#${confirmDeleteSpan}`).hide();
    }
}

function uuidv4() {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0;
        const v = c === "x" ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

$(document).ready(() => {
    
    // #region DELETE BUtton CONFRIMATION

    $(".my-confirm-delete").on("click", e => {
        const $btn = $(e.target);
        const id = $btn.attr("my-unique-id");
        const isDeleteClicked = $btn.attr("my-is-delete-clicked") === "true";

        confirmDelete(id, isDeleteClicked);
    });

    // #endregion

    // #region UPLOAD CONTROL

    $("button:contains('Browse')").on("click", () => {
        $("input[type='file']").trigger("click");
    });

    $("input[type='file']").on("change", e => {
        const $fuControl = $(e.target);
        const $label = $fuControl.prev("input[type='text']");
        const $thumbnail = $(".image-thumbnail");
        const files = $fuControl[0].files;

        if (files.length > 1) {
            $label.val(`${files.length} files selected`);
        } else if (files.length === 1) {
            const fileName = $fuControl.val().split("\\").pop();
            $label.val(fileName);
        }

        var url = URL.createObjectURL(files[0]);
        $thumbnail.attr("src", url);
    });

    // #endregion

    // #region SELECT CONTROL FORMATTING

    $("select").on("mousedown", e => {
        e.preventDefault();
        if (e.which !== 1) {
            return false;
        }

        const $select = $(e.target);
        let guid = uuidv4();
        if (!$select.attr("guid")) {
            $select.attr("guid", guid);
        } else {
            guid = $select.attr("guid");
        }

        $select.focus();
        const $selectParent = $select.parent();

        let $ulOptionsContainer = $selectParent.children(".my-select-options-container").toArray().map(el => $(el)).filter($el => $el.attr("guid") === $select.attr("guid"))[0] || null;
        const hiding = $ulOptionsContainer !== null;

        if (!$ulOptionsContainer) {
            $ulOptionsContainer = $(`<ul class='my-select-options-container' guid='${guid}'></ul>`);
            const $options = $select.children("option").toArray().map(el => $(el)).filter($el => $el.text().toLowerCase() !== "none");

            for (let $option of $options) {
                $ulOptionsContainer.append(`<li class='my-select-option' value='${$option.val()}'>${$option.text()}</li>`);
            }

            const $parent = $select.parent();
            $parent.attr("position", "relative");
            $parent.append($ulOptionsContainer);
            $ulOptionsContainer.css("left", $select.position().left);
            $ulOptionsContainer.css("top", $select.position().top + $select.outerHeight());
            $ulOptionsContainer.css({
                "width": $select.outerWidth(false) + "px",
                "display": "none"
            });
        }

        $ulOptionsContainer.stop(true, true).animate({
            height: ["toggle", "swing"],
            opacity: "toggle"
        }, 250, "linear", function () {
            if (hiding) {
                $ulOptionsContainer.remove();
            }
        });
    });

    $(window).on("resize", e => {
        const $selectOptionContainers = $(".my-select-options-container").toArray().map(el => $(el));

        for (let $optionContainer of $selectOptionContainers) {
            const $select = $optionContainer.parent().children("select").toArray().map(el => $(el))
                .filter($el => $el.attr("guid") === $optionContainer.attr("guid"))[0];
            $optionContainer.css({
                "width": $select.outerWidth(false) + "px",
                "left": $select.position().left,
                "top": $select.position().top + $select.outerHeight()
            });
        };

    });

    $(document).on("mousedown", ".my-select-option", e => {
        if (e.which !== 1) {
            return false;
        }

        const $option = $(e.target);
        const val = $option.attr("value");
        const $ulOptionsContainer = $option.parent();
        const $select = $ulOptionsContainer.parent().children("select").toArray().map(el => $(el))
            .filter($el => $el.attr("guid") === $ulOptionsContainer.attr("guid"))[0];
        const $selectOptions = $select.children("option").toArray().map(el => $(el));

        for (let $selectOption of $selectOptions) {
            if (val === $selectOption.val()) {
                $selectOption.attr("selected", "selected");
            } else {
                $selectOption.removeAttr("selected");
            }
        }

        $ulOptionsContainer.stop(true, true).animate({
            height: ["hide", "swing"],
            opacity: "hide"
        }, 250, "linear", function () {
            $ulOptionsContainer.remove();
        });

        $select.focus();
        e.preventDefault();
    });

    // #endregion

});