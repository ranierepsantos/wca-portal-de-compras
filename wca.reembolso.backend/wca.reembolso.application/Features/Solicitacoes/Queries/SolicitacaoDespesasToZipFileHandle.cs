using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using wca.reembolso.application.Common;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Queries
{
    public sealed record SolicitacaoDespesasToZipFileQuery(int Id) : IRequest<ErrorOr<Stream>>;
    internal class SolicitacaoDespesasToZipFileHandle : IRequestHandler<SolicitacaoDespesasToZipFileQuery, ErrorOr<Stream>>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SolicitacaoDespesasToZipFileHandle> _logger;

        public SolicitacaoDespesasToZipFileHandle(ILogger<SolicitacaoDespesasToZipFileHandle> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<Stream>> Handle(SolicitacaoDespesasToZipFileQuery request, CancellationToken cancellationToken)
        {
            try
            {

                // localizar cliente
                var querie = new SolicitacaoByIdQuerie(request.Id);

                var findResult = await _mediator.Send(querie, cancellationToken);

                if (findResult.IsError) return findResult.FirstError;

                Dictionary<int,string> files = new();

                foreach (Despesa despesa in findResult.Value.Despesa)
                {
                    if (!string.IsNullOrEmpty(despesa.ImagePath))
                        files.TryAdd(despesa.Id, despesa.ImagePath.Replace(MyHttpContext.AppBaseUrl, "wwwroot"));
                }

                if (files.Count > 0)
                {
                    var memoryStream = new MemoryStream();
                    
                    // Create a new zip archive
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in files)
                        {
                            var fileInfo = new FileInfo(file.Value);

                            // Create a new entry in the zip archive for each file
                            var entry = zipArchive.CreateEntry($"comprovante_despesa_{file.Key}{fileInfo.Extension}");

                            // Write the file contents into the entry
                            using var entryStream = entry.Open();
                            using var fileStream = new FileStream(file.Value, FileMode.Open, FileAccess.Read);
                            fileStream.CopyTo(entryStream);
                        }
                    }
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return memoryStream;
                }
                return Error.Failure(description: "Não havia arquivos para download!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Error.Failure(description: ex.Message);
            }
        }
    }

    /*
     // Create a temporary memory stream to hold the zip archive
        using(var memoryStream = new MemoryStream()) {
          // Create a new zip archive
          using(var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true)) {
            foreach(var file in files) {
              var fileInfo = new FileInfo(file);

              // Create a new entry in the zip archive for each file
              var entry = zipArchive.CreateEntry(fileInfo.Name);

              // Write the file contents into the entry
              using(var entryStream = entry.Open())
              using(var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                fileStream.CopyTo(entryStream);
              }
            }
          }

          memoryStream.Seek(0, SeekOrigin.Begin);

          // Return the zip file as a byte array
          return File(memoryStream.ToArray(), "application/zip", "files.zip");
     */
}
