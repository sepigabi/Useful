//Service.ts

downloadTemplateFile(templateId: number): Observable<Blob> {
        let params = { templateId: templateId };
        return this._http.get(this._configService.getConfigValue("BASE_USER_ENDPOINT") + "DownloadTemplateFile/", { params: params, responseType: ResponseContentType.Blob })
            .map((respone: Response) => respone.blob())
            .catch(this.handleError);
    }

createTemplate(template: FormData) {
        return this._http.put(this._configService.getConfigValue("BASE_USER_ENDPOINT") + "CreateTemplate/", template)
            .catch(this.handleError);
    }

------------------------------------------------------------------------------------------------------------------------------------------
//Component.ts

addNewTemplateForm() {
        this.busy = true;
        const formData: FormData = new FormData();
        formData.append("templateTypeId", this.documentIDCombo.value);
        formData.append("documentVersion", this.documentVersionInput.value);
        formData.append("formFile", this.fileToUpload);

        this._templateService.createTemplate(formData).subscribe(() => {
            this.resetNewTemplateForm();
            this.refreshTemplatesFromDatabase();
        });
    }

    downLoadFile(templateId: number) {
        this.busy = true;
        const template = this.originalItems.filter(t => t.Id === templateId)[0];
        this._templateService.downloadTemplateFile(templateId).subscribe((result) => {
            const blob = new Blob([result], { type: 'application/pdf' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = template.FileName;
            link.click();
            window.URL.revokeObjectURL(link.href);
            this.busy = false;
        });
    }

    viewFile(templateId: number) {
        this.busy = true;
        const template = this.originalItems.filter(t => t.Id === templateId)[0];
        this._templateService.downloadTemplateFile(templateId).subscribe((result) => {
            const blob = new Blob([result], { type: 'application/pdf' });
            const url = window.URL.createObjectURL(blob);
            setTimeout(() => {
                window.open(url);
            }, 1)
            this.busy = false;
        });
    }

    printFile(templateId: number) {
        this.busy = true;
        const template = this.originalItems.filter(t => t.Id === templateId)[0];
        this._templateService.downloadTemplateFile(templateId).subscribe((result) => {
            const blob = new Blob([result], { type: 'application/pdf' });
            const url = window.URL.createObjectURL(blob);
            setTimeout(() => {
                const newWindow = window.open(url);
                newWindow.print();
            }, 1)
            this.busy = false;
        });
    }

--------------------------------------------------------------------------------------------------------------------------------------
Controller.cs

[Route("DownloadTemplateFile")]
[HttpGet]
public IHttpActionResult DownloadTemplateFile(int templateId)
{
    try
    {
      var templateFile = FormFillerClient.Instance.GetTemplateFile(templateId);
      HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
      responseMessage.Content = new ByteArrayContent(templateFile.FileContent);
      responseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
      responseMessage.Content.Headers.ContentDisposition.FileName = templateFile.FileName;
      responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
      return ResponseMessage(responseMessage);
    }
    catch (System.Exception e)
    {
      return InternalServerError(e);
    }
}

[Route("CreateTemplate")]
[HttpPut]
public IHttpActionResult CreateTemplate()
{
  try
  {
    var createTemplateDTO = new CreateTemplateDTO()
    {
      TemplateTypeId = int.Parse(HttpContext.Current.Request.Params["templateTypeId"]),
      DocumentVersion = HttpContext.Current.Request.Params["documentVersion"],
      FormFile = HttpContext.Current.Request.Files["formFile"],
      CreatedBy = HttpContext.Current.User.Identity?.Name
    };
    FormFillerClient.Instance.CreateTemplate(createTemplateDTO);
    return Ok();
    }
    catch (System.Exception e)
    {
      return InternalServerError(e);
    }
}
