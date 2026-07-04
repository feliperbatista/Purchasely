using Purchasely.Domain.Enums;

namespace Purchasely.Application.DTOs;

public record ReceiptFileDto(
    string FileName,
    string ContentType,
    long FileSizeBytes,
    Stream Stream
);