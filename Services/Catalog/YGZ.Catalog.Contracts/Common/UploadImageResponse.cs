

namespace YGZ.Catalog.Contracts.Common;

public sealed record UploadImageResponse(
                                        string Original_filename,
                                        string Format,
                                        string Public_id,
                                        string Display_name,
                                        string Secure_url,
                                        decimal Length,
                                        decimal Bytes,
                                        decimal Width,
                                        decimal Height) { }

