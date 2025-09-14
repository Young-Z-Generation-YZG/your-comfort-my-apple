

namespace YGZ.Catalog.Application.Common.Commands;

public sealed record ImageCommand(string ImageId,
                                  string ImageUrl,
                                  string ImageName,
                                  string ImageDescription,
                                  decimal ImageWidth,
                                  decimal ImageHeight,
                                  decimal ImageBytes,
                                  int Order)
{ }

