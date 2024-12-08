type DragWrapperProps = {
  className?: string;
  children?: React.ReactNode;
  renderDragBtn?: () => React.ReactElement;
  renderDeleteBtn?: () => React.ReactElement;
  addMore?: () => void;
};

const DragWrapper = (props: DragWrapperProps) => {
  return (
    <div className={props.className}>
      <div className="flex gap-5">
        <div className="flex-1">{props.children}</div>
        <div>
          <div className="flex gap-2 translate-y-[100%]">
            {props.renderDeleteBtn && props.renderDeleteBtn()}
            {props.renderDragBtn && props.renderDragBtn()}
          </div>
        </div>
      </div>

      {/* {props.addMore && (
        <div className="flex justify-end mt-2">
          <button
            className="text-primary-500 hover:text-primary-700"
            onClick={props.addMore}
          >
            Add More
          </button>
        </div>
      )} */}
    </div>
  );
};

export default DragWrapper;
