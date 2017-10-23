// This class manages signing and watermark requests
function PageContext() {
    this.init = function (serviceUrl) {
        this.baseUrl = serviceUrl;
        this.apiUrl = this.buildUrl(this.baseUrl, "api");
        this.watermarkApiUrl = this.buildUrl(this.apiUrl, "watermark");
        this.signingApiUrl = this.buildUrl(this.apiUrl, "signature");
        this.keysApiUrl = this.buildUrl(this.apiUrl, "signingkeys");
        this.inProgressState = "In progress";
        this.finishedState = "Finished";
        this.failedState = "Failed";
        this.resultsContainer = $("#resultsContainer");
        this.operationStatus = $("#operationStatus");
        this.downloadLinksContainer = $("#downloadLinksContainer");
        this.errorContentContainer = $("#errorContentContainer");
        this.actionsContainer = $("#actionsContainer");
        this.sourceFilesInput = $("#sourceFiles");
        this.watermarkImage = $("#watermarkImage");
        this.proceedWithWatermarkButton = $("#proceedWithWatermark");
        this.loadingBackdrop = $("#loadingBackdrop");
        this.watermarkText = $("#watermarkText");
        this.existingKeysSelect = $("#existingKeys");
        this.signingCert = $("#signingCert");
        this.signatureImage = $("#signatureImage");
        this.signatureText = $("#signatureText");
        this.signatureLeft = $("#signatureLeft");
        this.signatureRight = $("#signatureRight");
        this.signatureBottom = $("#signatureBottom");
        this.signatureTop = $("#signatureTop");
        this.signatureKeyPassword = $("#signingKeyPassword");
        this.proceedWithSignatureButton = $("#proceedWithSignature");
        this.serviceUrlAndStatus = $("#serviceUrlAndStatus");
        this.taskFailedMessage = "Failed to get task results";
        this.startTaskFirst = "You need to start a task first";

        // save reference
        var self = this;

        // source files on change handler
        this.sourceFilesInput.on("change",
            function () {
                if (this.files.length > 0) {
                    self.actionsContainer.show();
                } else {
                    self.actionsContainer.hide();
                }
            });

        // watermark text or image change handler
        var watermarkValidator = () => self.validateWatermarkParameters();

        this.watermarkText.on("input", watermarkValidator);
        this.watermarkImage.on("change", watermarkValidator);

        // signature validator
        var signatureValidator = () => self.validateSignatureParameters();

        this.existingKeysSelect.on("change", signatureValidator);
        this.signingCert.on("change", signatureValidator);
        this.signatureLeft.on("input", signatureValidator);
        this.signatureRight.on("input", signatureValidator);
        this.signatureTop.on("input", signatureValidator);
        this.signatureBottom.on("input", signatureValidator);

        // set watermark button click handler
        this.proceedWithWatermarkButton.on("click",
            function (e) {
                // show the modal
                self.showModalLoadingBackdrop(true);

                self.resetErrors();
                // hide results
                self.downloadLinksContainer.parent().hide();

                // init form data for sending
                var watermarkImageData = self.watermarkImage[0].files;
                var filesToSend = self.sourceFilesInput[0].files;

                var formData = new FormData();
                formData.append("watermarkText", self.watermarkText.val());
                formData.append("watermarkImage", watermarkImageData[0]);

                $.each(filesToSend,
                    function (index, value) {
                        formData.append("sourceFiles" + index, value);
                    });

                // call watermark api
                $.ajax({
                    url: self.watermarkApiUrl,
                    data: formData,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    success: function (data) {
                        self.getResults(self.watermarkApiUrl, data);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        self.reportError("Error occured while contacting the server: " + textStatus);
                        self.requestFinishedCallback();
                    }
                });
            });

        // set signing button click handler
        this.proceedWithSignatureButton.on("click",
            function (e) {
                // show the modal
                self.showModalLoadingBackdrop(true);

                self.resetErrors();

                // hide results
                self.downloadLinksContainer.parent().hide();

                // init form data for sending
                var signatureImageData = self.signatureImage[0].files;
                var filesToSend = self.sourceFilesInput[0].files;
                var signingCertData = self.signingCert[0].files;

                var formData = new FormData();
                formData.append("signatureText", self.signatureText.val());
                formData.append("signatureImage", signatureImageData[0]);
                formData.append("signingKeyName", self.existingKeysSelect.val());
                formData.append("signingCertificate", signingCertData[0]);
                formData.append("signingPassword", self.signatureKeyPassword.val());
                formData.append("signatureBoundary",
                    self.signatureLeft.val() +
                    ',' +
                    self.signatureBottom.val() +
                    ',' +
                    self.signatureRight.val() +
                    ',' +
                    self.signatureTop.val());
                formData.append("signaturePageIndexStart", 0);
                formData.append("signaturePageIndexEnd", 0);

                $.each(filesToSend,
                    function (index, value) {
                        formData.append("sourceFiles" + index, value);
                    });

                // call signing api
                $.ajax({
                    url: self.signingApiUrl,
                    data: formData,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    success: function (data) {
                        self.getResults(self.signingApiUrl, data);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        self.reportError("Error occured while contacting the server: " + textStatus);
                        self.requestFinishedCallback();
                    }
                });
            });

        // call keys api for loading existing keys
        self.loadExistingKeyInfo();
    }

    this.loadExistingKeyInfo = function () {
        var self = this;
        var statusText = $("<a>", { href: self.baseUrl, text: self.baseUrl });

        $.ajax({
            url: self.keysApiUrl,
            type: 'GET',
            success: function (data) {
                if (data) {
                    // insert empty option
                    self.existingKeysSelect.append($("<option>", { text: "", value: "" }));
                    // add found keys
                    $.each(data,
                        function (index, keyName) {
                            self.existingKeysSelect.append($("<option>", { text: keyName, value: keyName }));
                        });

                    //self.serviceUrlAndStatus.html("Connected to: ");
                    //self.serviceUrlAndStatus.append(statusText);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                self.existingKeysSelect.prop("disabled", true);
                // self.serviceUrlAndStatus.html("Service is unavailable: ");
                //self.serviceUrlAndStatus.append(statusText);
            }
        });
    }

    // "this" will be PageContext
    this.validateWatermarkParameters = function () {
        if (this.watermarkText.val() || this.watermarkImage[0].files.length > 0) {
            this.proceedWithWatermarkButton.prop("disabled", false);
        } else {
            this.proceedWithWatermarkButton.prop("disabled", true);
        }
    }

    // "this" will be PageContext
    this.validateSignatureParameters = function () {
        if ((this.existingKeysSelect.val() || this.signingCert[0].files.length > 0) &&
        (this.signatureLeft.val() &&
            this.signatureBottom.val() &&
            this.signatureTop.val() &&
            this.signatureRight.val())) {
            this.proceedWithSignatureButton.prop("disabled", false);
        } else {
            this.proceedWithSignatureButton.prop("disabled", true);
        }
    }

    this.showModalLoadingBackdrop = function (show) {
        if (show) {
            this.loadingBackdrop.appendTo(document.body).fadeIn();
        } else {
            this.loadingBackdrop.hide();
        }
    }

    // when the request is finished with any result this callback is called
    this.requestFinishedCallback = function () {
        this.showModalLoadingBackdrop(false);
    }

    // builds a compound url
    this.buildUrl = function () {
        if (arguments.length > 0) {
            var result = arguments[0];
            for (var i = 1; i < arguments.length; i++) {
                result += "/" + arguments[i];
            }
            return result;
        }

        return "";
    }

    this.resetErrors = function () {
        this.errorContentContainer.empty();
        this.errorContentContainer.parent().hide();
    }

    this.reportError = function (errorMessage) {
        this.errorContentContainer.empty();
        this.errorContentContainer.append($("<span>", { text: errorMessage }));
        this.errorContentContainer.parent().show();
    }

    // api polling function that is responsible for getting results and 
    // execution status
    this.getResults = function (apiUrl, taskId) {
        var self = this;
        if (taskId != null) {
            $.ajax({
                url: self.buildUrl(apiUrl, taskId),
                type: 'GET',
                success: function (data, textStatus, request) {

                    // finished OK: create download links
                    if (data.state == self.finishedState) {

                        // display the results container
                        self.downloadLinksContainer.empty();

                        // create button links for resulting files
                        var toolbar = $("<div>").attr("class", "btn-toolbar");

                        for (var i = 0; i < data.fileNames.length; i++) {
                            self.downloadLinksContainer.append(toolbar.append($("<div>")
                                .attr("class", "btn-group")
                                .append($("<a>")
                                    .attr("href", self.buildUrl(apiUrl, taskId, i))
                                    .attr("class", "btn btn-info btn-lg")
                                    .attr("role", "button")
                                    .text(data.fileNames[i]))));

                        }

                        self.downloadLinksContainer.parent().show();
                    } else {
                        // report state and initiate new request if needed
                        self.operationStatus.text(data.state);

                        // set up the callback to query the state again
                        if (data.state == self.inProgressState) {
                            setTimeout(function () { self.getResults(apiUrl, taskId) }, 500);
                            return;
                        }

                        if (data.state == self.failedState) {
                            self.reportError(self.taskFailedMessage);
                        }
                    }

                    self.requestFinishedCallback();
                },
                error: function (xhr, textStatus, thrownError) {
                    self.reportError(textStatus);
                    self.requestFinishedCallback();
                }
            });
        } else {
            self.reportError(self.startTaskFirst);
        }
    };
}



