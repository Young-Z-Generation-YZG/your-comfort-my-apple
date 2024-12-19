import {
  ICloudinaryImage,
  IUploadPayload,
} from "~/types/common/cloudinary-image.type";
import { get, post } from "~/utils/http-request";

export const getImagesAsync = async (): Promise<ICloudinaryImage[]> => {
  const response = await get("upload/images");

  return response;
};

export const uploadMultipleImage = async (payload: IUploadPayload) => {
  const formData = new FormData();

  payload.images.forEach((image) => formData.append("images", image));

  // const result = await post("upload/multiple", formData);
};
