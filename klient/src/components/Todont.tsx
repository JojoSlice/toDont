export { default as TodontList } from "./TodontList";
export type TodontProps = {
  title: string;
};

export default function Todont({ title }: TodontProps) {
  return (
    <li className="todont">
      <input type="checkbox" />
      <span>{title}</span>
      <button>X</button>
    </li>
  );
}
