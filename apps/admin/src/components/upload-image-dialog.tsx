/* eslint-disable @next/next/no-img-element */
"use client";

import { useState, useRef } from "react";
import { Button } from "@components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@components/ui/dialog";
import { Input } from "@components/ui/input";
import { Label } from "@components/ui/label";
import { X, Upload, ImageIcon } from "lucide-react";
import { toast } from "~/src/hooks/use-toast";

export default function UploadImageDialog({
  maxFiles = 5,
  maxFileSize = 5 * 1024 * 1024, // 5MB
  acceptedFileTypes = ["image/jpeg", "image/png", "image/webp"],
  triggerBtnRef = null,
}: {
  maxFiles?: number;
  maxFileSize?: number;
  acceptedFileTypes?: string[];
  triggerBtnRef?: React.RefObject<HTMLButtonElement> | null;
}) {
  const [files, setFiles] = useState<File[]>([]);
  const [previews, setPreviews] = useState<string[]>([]);
  const [dragActive, setDragActive] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  const [loading, setLoading] = useState(false);

  const handleDrag = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    if (e.type === "dragenter" || e.type === "dragover") {
      setDragActive(true);
    } else if (e.type === "dragleave") {
      setDragActive(false);
    }
  };

  const handleDrop = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);
    if (e.dataTransfer.files && e.dataTransfer.files[0]) {
      handleFiles(e.dataTransfer.files);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();
    if (e.target.files && e.target.files[0]) {
      handleFiles(e.target.files);
    }
  };

  const handleFiles = (inputFiles: FileList) => {
    setError(null);
    const newFiles = Array.from(inputFiles).filter((file) =>
      acceptedFileTypes.includes(file.type)
    );

    if (files.length + newFiles.length > maxFiles) {
      setError(`You can only upload a maximum of ${maxFiles} files.`);
      return;
    }

    const oversizedFiles = newFiles.filter((file) => file.size > maxFileSize);
    if (oversizedFiles.length > 0) {
      setError(
        `Some files exceed the maximum file size of ${maxFileSize / 1024 / 1024
        }MB.`
      );
      return;
    }

    setFiles((prevFiles) => [...prevFiles, ...newFiles]);

    // Generate previews
    newFiles.forEach((file) => {
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviews((prevPreviews) => [
          ...prevPreviews,
          reader.result as string,
        ]);
      };
      reader.readAsDataURL(file);
    });
  };

  const removeFile = (index: number) => {
    setFiles((prevFiles) => prevFiles.filter((_, i) => i !== index));
    setPreviews((prevPreviews) => prevPreviews.filter((_, i) => i !== index));
  };

  const onButtonClick = () => {
    inputRef.current?.click();
  };

  const handleUpload = async () => {
    toast({
      type: "background",
      title: "Not Implemented",
      variant: "destructive",
      description: "Function not implement yet",
    });

    // setLoading(true);
    // const result = await UploadServices.uploadMultipleImage({
    //   images: files,
    // });
    // setLoading(false);
    // Reset the state after upload
    // setFiles([]);
    // setPreviews([]);
    // window.location.reload();
  };

  return (
    <Dialog>
      <DialogTrigger asChild ref={triggerBtnRef}>
        <Button
          variant="outline"
          className="px-2 py-1 text-xs h-7 dark:bg-four dark:hover:bg-slate-700 hidden"
          onClick={() => {
            if (triggerBtnRef?.current) triggerBtnRef.current?.click();
          }}
        >
          Upload Images
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px] rounded-md">
        <DialogHeader>
          <DialogTitle className="">Upload Images</DialogTitle>
        </DialogHeader>
        <div
          className={`p-4 mt-4 border-2 border-dashed rounded-lg ${dragActive ? "border-primary" : "border-gray-300"
            }`}
          onDragEnter={handleDrag}
          onDragLeave={handleDrag}
          onDragOver={handleDrag}
          onDrop={handleDrop}
        >
          <div className="flex flex-col items-center justify-center py-5">
            <Upload className="w-10 h-10 text-gray-400" />
            <p className="mt-2 text-sm text-gray-500">
              Drag and drop your images here, or click to select files
            </p>
            <p className="mt-1 text-xs text-gray-500">
              {acceptedFileTypes.join(", ")} files are allowed (max {maxFiles}{" "}
              files, {maxFileSize / 1024 / 1024}MB each)
            </p>
            <Button type="button" onClick={onButtonClick} className="mt-2">
              Select Files
            </Button>
            <Input
              ref={inputRef}
              type="file"
              multiple
              accept={acceptedFileTypes.join(",")}
              onChange={handleChange}
              className="hidden"
            />
          </div>
        </div>
        {error && <p className="mt-2 text-sm text-red-500">{error}</p>}
        {files.length > 0 && (
          <div className="mt-4">
            <Label>Uploaded Files:</Label>
            <ul className="mt-2 space-y-2">
              {files.map((file, index) => (
                <li
                  key={index}
                  className="flex items-center justify-between p-2 rounded bg-slate-700"
                >
                  <div className="flex items-center space-x-2">
                    {previews[index] ? (
                      <img
                        src={previews[index]}
                        alt={`Preview of ${file.name}`}
                        className="object-cover w-10 h-10 rounded"
                      />
                    ) : (
                      <ImageIcon className="w-10 h-10 text-gray-400" />
                    )}
                    <span className="text-sm truncate">{file.name}</span>
                  </div>
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    onClick={() => removeFile(index)}
                  >
                    <X className="w-4 h-4" />
                    <span className="sr-only">Remove file</span>
                  </Button>
                </li>
              ))}
            </ul>
          </div>
        )}
        {files.length > 0 && (
          <Button onClick={handleUpload} className="w-full mt-4">
            Upload {files.length} {files.length === 1 ? "File" : "Files"}
          </Button>
        )}
      </DialogContent>
    </Dialog>
  );
}
